const express = require('express');
const router = express.Router();
const Company = require('../models/Company');

// GET /api/companies - Récupérer toutes les companies
router.get('/', async (req, res) => {
  try {
    const companies = await Company.find().sort({ createdAt: -1 });
    
    res.json({
      success: true,
      data: companies,
      count: companies.length
    });
  } catch (error) {
    console.error('❌ Erreur lors de la récupération des entreprises:', error);
    res.status(500).json({
      success: false,
      error: 'Internal server error',
      message: error.message
    });
  }
});

// GET /api/companies/:id - Récupérer une company par ID
router.get('/:id', async (req, res) => {
  try {
    const companyId = req.params.id;
    console.log('📥 GET /api/companies/:id - ID reçu:', companyId);
    
    const company = await Company.findById(companyId);

    if (!company) {
      return res.status(404).json({
        success: false,
        error: 'Not Found',
        message: `Company with ID ${companyId} not found`
      });
    }

    console.log('✅ Company trouvée:', company.name);
    res.json({
      success: true,
      data: company
    });

  } catch (error) {
    console.error('❌ Erreur lors de la récupération de l\'entreprise:', error);
    
    // Gestion des erreurs MongoDB (ID invalide)
    if (error.name === 'CastError') {
      return res.status(400).json({
        success: false,
        error: 'Bad Request',
        message: 'Invalid company ID format'
      });
    }
    
    res.status(500).json({
      success: false,
      error: 'Internal server error',
      message: error.message
    });
  }
});

// POST /api/companies - Créer une nouvelle company
router.post('/', async (req, res) => {
  try {
    const companyData = req.body;
    
    // Log du payload reçu pour debug
    console.log('📥 POST /api/companies - Payload reçu:', JSON.stringify(companyData, null, 2));

    // Validation de base
    if (!companyData || Object.keys(companyData).length === 0) {
      console.error('❌ BadRequest: Request body is required or empty');
      return res.status(400).json({
        success: false,
        error: 'Bad Request',
        message: 'Request body is required'
      });
    }
    
    // Vérifier si l'entreprise existe déjà
    const existing = await Company.findByName(companyData.name);
    if (existing) {
      console.log('🔄 Company déjà existante, on la retourne:', companyData.name);
      return res.status(200).json({
        success: true,
        message: 'Company already exists',
        data: existing
      });
    }
    
    // Créer la nouvelle entreprise
    const newCompany = new Company(companyData);
    await newCompany.save();
    
    console.log('✅ Company créée avec succès:', newCompany.name);

    res.status(201).json({
      success: true,
      message: 'Company created successfully',
      data: newCompany
    });
    
  } catch (error) {
    console.error('❌ Erreur lors de la création de company:', error.message);
    
    // Gestion des erreurs de validation Mongoose
    if (error.name === 'ValidationError') {
      const errors = Object.values(error.errors).map(e => e.message);
      return res.status(400).json({
        success: false,
        error: 'Validation Error',
        message: errors.join(', ')
      });
    }
    
    // Gestion des erreurs de duplication (nom unique)
    if (error.code === 11000) {
      return res.status(409).json({
        success: false,
        error: 'Conflict',
        message: 'Une entreprise avec ce nom existe déjà'
      });
    }

    res.status(500).json({
      success: false,
      error: 'Internal server error',
      message: error.message
    });
  }
});

// GET /api/companies/metadata/industries - Récupérer les secteurs d'activité
router.get('/metadata/industries', async (req, res) => {
  try {
    const industries = await Company.distinct('industry');
    
    res.json({
      success: true,
      data: industries.sort()
    });
  } catch (error) {
    console.error('❌ Erreur lors de la récupération des secteurs:', error);
    res.status(500).json({
      success: false,
      error: 'Internal server error',
      message: error.message
    });
  }
});

// GET /api/companies/metadata/sizes - Récupérer les tailles d'entreprise
router.get('/metadata/sizes', async (req, res) => {
  try {
    const sizes = ['1-10', '11-50', '51-200', '201-500', '501-1000', '1000+'];
    
    res.json({
      success: true,
      data: sizes
    });
  } catch (error) {
    console.error('❌ Erreur lors de la récupération des tailles:', error);
    res.status(500).json({
      success: false,
      error: 'Internal server error',
      message: error.message
    });
  }
});

module.exports = router; 