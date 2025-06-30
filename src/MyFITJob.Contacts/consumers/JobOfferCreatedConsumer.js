const amqp = require('amqplib');
const Company = require('../models/Company');
const CompanyCreatedPublisher = require('../publishers/CompanyCreatedPublisher');

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
        this.publisher = null;
        this.queueName = 'job-offer-created';
        this.rabbitMqUrl = process.env.RABBITMQ_URL || 'amqp://rabbitmq:5672';
        this.exchangeName = 'MyFITJob.Api.Messaging.Contracts:JobOfferCreatedEvent';
        this.exchangeType = 'fanout';
    }

    /**
     * Initialise la connexion RabbitMQ et configure le consumer
     */
    async initialize() {
        try {
            console.log('🔌 Connexion à RabbitMQ...');
            
            // Attendre un peu pour laisser RabbitMQ finir de démarrer
            await new Promise(resolve => setTimeout(resolve, 2000));
            
            // Établir la connexion à RabbitMQ
            this.connection = await amqp.connect(this.rabbitMqUrl);
            
            // Créer un canal de communication
            this.channel = await this.connection.createChannel();
            
            // Initialiser le publisher avec le même canal
            this.publisher = new CompanyCreatedPublisher(this.channel);
            await this.publisher.initialize();
            
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
            
            console.log(`🏢 Traitement de l'entreprise: ${companyName} pour l'offre ${jobOfferId}`);
            
            // Vérifier si l'entreprise existe déjà
            const existingCompany = await Company.findByName(companyName);
            
            let companyToPublish;
            
            if (existingCompany) {
                console.log(`ℹ️ Entreprise "${companyName}" existe déjà (ID: ${existingCompany.id})`);
                companyToPublish = existingCompany;
            } else {
                // Créer automatiquement l'entreprise dans MongoDB
                const companyData = {
                    name: companyName,
                    industry: industry || 'Unknown',
                    size: size || '1-50',
                    rating: 0,
                    description: `Entreprise créée automatiquement pour l'offre ${jobOfferId}`
                };
                
                const newCompany = new Company(companyData);
                await newCompany.save();
                
                console.log(`✅ Entreprise "${companyName}" créée automatiquement en MongoDB (ID: ${newCompany.id})`);
                console.log(`📊 Détails: Industry=${industry}, Size=${size}`);
                companyToPublish = newCompany;
            }
            
            // Publier TOUJOURS l'événement CompanyCreated (création OU entreprise existante)
            await this.publisher.publishCompanyCreated(companyToPublish, jobOfferId);
            
        } catch (error) {
            console.error('❌ Erreur lors du traitement du message:', error);
            
            // Gestion spécifique des erreurs MongoDB
            if (error.name === 'ValidationError') {
                console.error('❌ Erreur de validation MongoDB:', error.message);
            } else if (error.code === 11000) {
                console.log('ℹ️ Entreprise déjà existante (contrainte d\'unicité)');
            }
            
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
