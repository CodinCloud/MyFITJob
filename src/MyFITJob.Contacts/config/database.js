const mongoose = require('mongoose');

class DatabaseConnection {
  constructor() {
    this.isConnected = false;
  }

  async connect() {
    try {
      const mongoUri = process.env.MONGODB_URI || 'mongodb://admin:adminpass@mongodb:27017/contactsdb?authSource=admin';
      
      console.log('🔌 Connexion à MongoDB...');
      
      // Configuration Mongoose
      await mongoose.connect(mongoUri, {
        maxPoolSize: 10, // Limite de connexions simultanées
        serverSelectionTimeoutMS: 5000, // Timeout de connexion
        socketTimeoutMS: 45000, // Timeout de socket
      });

      this.isConnected = true;
      console.log('✅ Connexion MongoDB établie avec succès !');
      
      // Écouter les événements de connexion
      this.setupEventListeners();
      
    } catch (error) {
      console.error('❌ Erreur de connexion MongoDB:', error.message);
      throw error;
    }
  }

  setupEventListeners() {
    mongoose.connection.on('connected', () => {
      console.log('📡 MongoDB connecté');
    });

    mongoose.connection.on('error', (err) => {
      console.error('❌ Erreur MongoDB:', err);
      this.isConnected = false;
    });

    mongoose.connection.on('disconnected', () => {
      console.log('📡 MongoDB déconnecté');
      this.isConnected = false;
    });

    // Gestion propre de la fermeture
    process.on('SIGINT', async () => {
      await this.disconnect();
      process.exit(0);
    });
  }

  async disconnect() {
    try {
      await mongoose.connection.close();
      console.log('🔌 Connexion MongoDB fermée');
      this.isConnected = false;
    } catch (error) {
      console.error('❌ Erreur lors de la fermeture MongoDB:', error);
    }
  }

  getConnectionStatus() {
    return {
      isConnected: this.isConnected,
      readyState: mongoose.connection.readyState,
      host: mongoose.connection.host,
      name: mongoose.connection.name
    };
  }
}

module.exports = new DatabaseConnection(); 