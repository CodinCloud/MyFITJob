const express = require('express');
const cors = require('cors');
const companiesRouter = require('./routes/companies');
const { initializeData } = require('./data/companies');

const app = express();
const PORT = process.env.PORT || 3002;

// Middleware
app.use(cors());
app.use(express.json());

// Routes
app.use('/api/companies', companiesRouter);

// Health check endpoint
app.get('/health', (req, res) => {
  res.json({ 
    status: 'OK', 
    service: 'MyFITJob.Contacts',
    timestamp: new Date().toISOString()
  });
});

// Root endpoint
app.get('/', (req, res) => {
  res.json({
    service: 'MyFITJob.Contacts',
    version: '1.0.0',
    description: 'REST API for company information management',
    endpoints: {
      health: '/health',
      companies: {
        getAll: 'GET /api/companies',
        getById: 'GET /api/companies/:id',
        create: 'POST /api/companies',
        metadata: {
          industries: 'GET /api/companies/metadata/industries',
          sizes: 'GET /api/companies/metadata/sizes'
        }
      }
    },
    examples: {
      createCompany: {
        method: 'POST',
        url: '/api/companies',
        body: {
          name: 'New Company',
          industry: 'Tech',
          size: '51-200',
          rating: 4.5,
          description: 'Company description'
        }
      }
    }
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

// Initialiser les données et démarrer le serveur
async function startServer() {
  try {
    // Initialiser les données persistantes
    await initializeData();
    
    app.listen(PORT, () => {
      console.log(`🚀 MyFITJob.Contacts microservice running on port ${PORT}`);
      console.log(`📊 Health check: http://localhost:${PORT}/health`);
      console.log(`🏢 Companies API: http://localhost:${PORT}/api/companies`);
      console.log(`📖 API Documentation: http://localhost:${PORT}/`);
    });
  } catch (error) {
    console.error('❌ Erreur lors du démarrage du serveur:', error);
    process.exit(1);
  }
}

startServer(); 