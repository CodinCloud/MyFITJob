const express = require('express');
const router = express.Router();
const { getCompanyById, getAllCompanies, createCompany, industries, companySizes } = require('../data/companies');

// GET /api/companies - Récupérer toutes les companies
router.get('/', (req, res) => {
  try {
    const companies = getAllCompanies();
    res.json({
      success: true,
      data: companies,
      count: companies.length
    });
  } catch (error) {
    res.status(500).json({
      success: false,
      error: 'Internal server error',
      message: error.message
    });
  }
});

// GET /api/companies/:id - Récupérer une company par ID
router.get('/:id', (req, res) => {
  try {
    const companyId = parseInt(req.params.id);
    
    if (isNaN(companyId)) {
      return res.status(400).json({
        success: false,
        error: 'Bad Request',
        message: 'Invalid company ID. Must be a number.'
      });
    }

    const company = getCompanyById(companyId);

    if (!company) {
      return res.status(404).json({
        success: false,
        error: 'Not Found',
        message: `Company with ID ${companyId} not found`
      });
    }

    // Retourner l'objet Company complet
    res.json(company);
  } catch (error) {
    res.status(500).json({
      success: false,
      error: 'Internal server error',
      message: error.message
    });
  }
});

// POST /api/companies - Créer une nouvelle company
router.post('/', (req, res) => {
  try {
    const companyData = req.body;

    // Validation de base
    if (!companyData || Object.keys(companyData).length === 0) {
      return res.status(400).json({
        success: false,
        error: 'Bad Request',
        message: 'Request body is required'
      });
    }

    const newCompany = createCompany(companyData);

    res.status(201).json({
      success: true,
      message: 'Company created successfully',
      data: newCompany
    });
  } catch (error) {
    // Gestion des erreurs de validation
    if (error.message.includes('required') || error.message.includes('Invalid') || error.message.includes('already exists')) {
      return res.status(400).json({
        success: false,
        error: 'Bad Request',
        message: error.message
      });
    }

    res.status(500).json({
      success: false,
      error: 'Internal server error',
      message: error.message
    });
  }
});

// GET /api/companies/metadata/industries - Récupérer les industries disponibles
router.get('/metadata/industries', (req, res) => {
  res.json({
    success: true,
    data: industries
  });
});

// GET /api/companies/metadata/sizes - Récupérer les tailles disponibles
router.get('/metadata/sizes', (req, res) => {
  res.json({
    success: true,
    data: companySizes
  });
});

module.exports = router; 