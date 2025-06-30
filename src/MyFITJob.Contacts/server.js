const express = require('express');
const cors = require('cors');
const companiesRouter = require('./routes/companies');
const database = require('./config/database');
const JobOfferCreatedConsumer = require('./consumers/JobOfferCreatedConsumer');

const app = express();
const PORT = process.env.PORT || 3002;

// Instance du consumer RabbitMQ
let jobOfferConsumer = null;

// Middleware
app.use(cors());
app.use(express.json());

// Routes
app.use('/api/companies', companiesRouter);

// Health check endpoint
app.get('/health', (req, res) => {
  const dbStatus = database.getConnectionStatus();
  
  res.json({ 
    status: 'OK', 
    service: 'MyFITJob.Contacts',
    timestamp: new Date().toISOString(),
    database: {
      connected: dbStatus.isConnected,
      readyState: dbStatus.readyState
    }
  });
});

// Root endpoint
app.get('/', (req, res) => {
  res.json({
    service: 'MyFITJob.Contacts',
    version: '2.0.0',
    description: 'REST API for company information management with MongoDB',
    database: 'MongoDB',
  });
});

// Error handling middleware
app.use((err, req, res, next) => {
  console.error(err.stack);
  res.status(500).json({
    success: false,
    error: 'Internal server error',
    message: 'Something went wrong!'
  });
});

// 404 handler
app.use('*', (req, res) => {
  res.status(404).json({
    success: false,
    error: 'Not found',
    message: `Route ${req.originalUrl} not found`
  });
});

// Fonction utilitaire pour retry avec délai exponentiel
async function retryWithBackoff(fn, maxRetries = 5, baseDelay = 1000) {
  for (let attempt = 1; attempt <= maxRetries; attempt++) {
    try {
      return await fn();
    } catch (error) {
      if (attempt === maxRetries) {
        throw error;
      }
      
      const delay = baseDelay * Math.pow(2, attempt - 1);
      console.log(`⏳ Tentative ${attempt}/${maxRetries} échouée, retry dans ${delay}ms...`);
      await new Promise(resolve => setTimeout(resolve, delay));
    }
  }
}

// Initialiser les données et démarrer le serveur
async function startServer() {
  try {
    // Connexion à MongoDB
    await database.connect();
    
    // Initialiser le consumer RabbitMQ avec retry
    jobOfferConsumer = new JobOfferCreatedConsumer();
    await retryWithBackoff(
      () => jobOfferConsumer.initialize(),
      5, // 5 tentatives maximum
      2000 // 2 secondes de délai initial
    );
    
    app.listen(PORT, () => {
      console.log(`🚀 MyFITJob.Contacts microservice running on port ${PORT}`);
      console.log(`📊 Health check: http://localhost:${PORT}/health`);
      console.log(`🏢 Companies API: http://localhost:${PORT}/api/companies`);
      console.log(`📖 API Documentation: http://localhost:${PORT}/`);
      console.log(`🐰 RabbitMQ Consumer: En attente de messages...`);
      console.log(`🍃 MongoDB: Connecté et opérationnel`);
    });
  } catch (error) {
    console.error('❌ Erreur lors du démarrage du serveur:', error);
    
    // Tenter de démarrer le serveur même si RabbitMQ n'est pas disponible
    console.log('🔄 Démarrage du serveur sans RabbitMQ...');
    app.listen(PORT, () => {
      console.log(`🚀 MyFITJob.Contacts microservice running on port ${PORT} (sans RabbitMQ)`);
      console.log(`📊 Health check: http://localhost:${PORT}/health`);
      console.log(`🏢 Companies API: http://localhost:${PORT}/api/companies`);
      console.log(`⚠️  RabbitMQ: Non disponible`);
    });
  }
}

startServer(); 

// Gestion propre de la fermeture
process.on('SIGINT', async () => {
  console.log('\n🛑 Arrêt du serveur...');
  
  // Fermer proprement le consumer RabbitMQ
  if (jobOfferConsumer) {
    await jobOfferConsumer.close();
  }
  
  // Fermer la connexion MongoDB
  await database.disconnect();
  
  process.exit(0);
});

process.on('SIGTERM', async () => {
  console.log('\n🛑 Arrêt du serveur (SIGTERM)...');
  
  // Fermer proprement le consumer RabbitMQ
  if (jobOfferConsumer) {
    await jobOfferConsumer.close();
  }
  
  // Fermer la connexion MongoDB
  await database.disconnect();
  
  process.exit(0);
}); 