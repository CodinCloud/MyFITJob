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

module.exports = router; 