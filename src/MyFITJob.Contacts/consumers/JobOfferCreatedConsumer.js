const amqp = require('amqplib');

/**
 * Consumer pour traiter les événements "JobOfferCreated"
 * 
 * Ce consumer écoute les messages RabbitMQ émis par l'API MyFITJob
 * quand une nouvelle offre d'emploi est créée.
 */
class JobOfferCreatedConsumer {
    constructor() {
        this.connection = null;
        this.channel = null;
        this.queueName = 'job-offer-created';
        this.rabbitMqUrl = process.env.RABBITMQ_URL || 'amqp://rabbitmq:5672';
        this.exchangeName = 'MyFITJob.Api.Messaging.Contracts:JobOfferCreated';
        this.exchangeType = 'fanout';
    }

    /**
     * Initialise la connexion RabbitMQ et configure le consumer
     */
    async initialize() {
        try {
            console.log('🔌 Connexion à RabbitMQ...');
            
            // Établir la connexion à RabbitMQ
            this.connection = await amqp.connect(this.rabbitMqUrl);
            
            // Créer un canal de communication
            this.channel = await this.connection.createChannel();
            
            // Déclarer l'exchange MassTransit (fanout)
            await this.channel.assertExchange(this.exchangeName, this.exchangeType, { durable: true });
            
            // Déclarer la queue (persistante)
            await this.channel.assertQueue(this.queueName, { durable: true });
            
            // Lier la queue à l'exchange
            await this.channel.bindQueue(this.queueName, this.exchangeName, '');
            
            console.log(`✅ Queue '${this.queueName}' liée à l'exchange '${this.exchangeName}'`);
            
            // Configurer le consumer
            this.channel.consume(this.queueName, async (message) => {
                if (message) {
                    try {
                        await this.processMessage(message);
                        // Acknowledge le message après traitement réussi
                        this.channel.ack(message);
                    } catch (error) {
                        console.error('❌ Erreur lors du traitement du message:', error);
                        // Rejeter le message en cas d'erreur
                        this.channel.nack(message, false, false);
                    }
                }
            });
            
        } catch (error) {
            console.error('❌ Erreur lors de l\'initialisation du consumer:', error);
            throw error;
        }
    }

    /**
     * Traite un message reçu de RabbitMQ
     * @param {Object} message - Le message reçu
     */
    async processMessage(message) {
        try {
            console.log('📨 Message reçu:', message.content.toString());
            
            // Parser le message JSON
            const jobOfferData = JSON.parse(message.content.toString());

            // MassTransit encapsule le message métier dans la propriété "message"
            const { jobOfferId, companyName, industry, size } = jobOfferData.message;
            
            console.log(`🏢 Création automatique de l'entreprise: ${companyName}`);
            
            // Créer automatiquement l'entreprise dans la base de données
            // Note: Ici on simule la création, en réalité vous importeriez vos services
            const companyData = {
                name: companyName,
                industry: industry || 'Unknown',
                size: size || '1-50', // Utilise la taille de l'offre d'emploi
                rating: 0,
                description: `Entreprise créée automatiquement pour l'offre ${jobOfferId}`
            };
            
            // TODO: Importer et utiliser votre service de création d'entreprise
            // const { createCompany } = require('../services/companyService');
            // await createCompany(companyData);
            
            console.log(`✅ Entreprise "${companyName}" créée automatiquement pour l'offre ${jobOfferId}`);
            console.log(`📊 Détails: Industry=${industry}, Size=${size}`);
            
        } catch (error) {
            console.error('❌ Erreur lors du traitement du message:', error);
            throw error; // Relancer l'erreur pour que le consumer la gère
        }
    }

    /**
     * Ferme proprement la connexion RabbitMQ
     */
    async close() {
        try {
            if (this.channel) {
                await this.channel.close();
            }
            if (this.connection) {
                await this.connection.close();
            }
            console.log('🔌 Connexion RabbitMQ fermée');
        } catch (error) {
            console.error('❌ Erreur lors de la fermeture:', error);
        }
    }
}

module.exports = JobOfferCreatedConsumer;
