/**
 * Publisher pour diffuser les événements "CompanyCreated"
 * 
 * Ce publisher diffuse un message RabbitMQ quand une entreprise est créée
 * pour informer les autres microservices (MyFITJob.Api, Frontend)
 */
const crypto = require('crypto');

class CompanyCreatedPublisher {
    constructor(channel) {
        this.channel = channel;
        this.exchangeName = 'MyFITJob.Api.Messaging.Contracts:CompanyCreatedEvent';
        this.exchangeType = 'fanout';
    }

    /**
     * Initialise l'exchange pour la publication
     */
    async initialize() {
        try {
            // Déclarer l'exchange MassTransit (fanout)
            await this.channel.assertExchange(this.exchangeName, this.exchangeType, { durable: true });
            console.log(`✅ Exchange '${this.exchangeName}' configuré pour publication`);
        } catch (error) {
            console.error('❌ Erreur lors de l\'initialisation du publisher:', error);
            throw error;
        }
    }

    /**
     * Publie un événement CompanyCreated
     * @param {Object} companyData - Les données de l'entreprise créée
     * @param {string} jobOfferId - L'ID de l'offre qui a provoqué la création
     */
    async publishCompanyCreated(companyData, jobOfferId) {
        try {
            const event = {
                messageId: this.generateMessageId(),
                timestamp: new Date().toISOString(),
                messageType: [
                    "urn:message:MyFITJob.Api.Messaging.Contracts:CompanyCreatedEvent"
                ],
                message: {
                    companyId: companyData.id || companyData._id.toString(),
                    jobOfferId: jobOfferId,
                    companyName: companyData.name,
                    industry: companyData.industry,
                    size: companyData.size,
                    createdAt: companyData.createdAt || new Date().toISOString()
                }
            };

            // Publier le message sur l'exchange
            const messageBuffer = Buffer.from(JSON.stringify(event));
            await this.channel.publish(this.exchangeName, '', messageBuffer, {
                persistent: true,
                contentType: 'application/json'
            });

            console.log(`📤 Événement CompanyCreatedEvent publié:`);
            console.log(`   🏢 Company: ${event.message.companyName} (ID: ${event.message.companyId})`);
            console.log(`   💼 JobOffer: ${event.message.jobOfferId}`);
            console.log(`   🏭 Industry: ${event.message.industry}`);

        } catch (error) {
            console.error('❌ Erreur lors de la publication CompanyCreated:', error);
            throw error;
        }
    }

    /**
     * Génère un ID unique pour le message au format GUID compatible MassTransit
     */
    generateMessageId() {
        // Utilise la méthode native Node.js pour générer un UUID v4
        return crypto.randomUUID();
    }
}

module.exports = CompanyCreatedPublisher; 