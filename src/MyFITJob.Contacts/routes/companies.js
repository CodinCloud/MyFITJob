const express = require('express');
const router = express.Router();
const { getCompanyById, getAllCompanies, createCompany, getCompanyByName } = require('../data/companies');

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
    console.log('📥 GET /api/companies/:id - Payload reçu:', JSON.stringify(company, null, 2));
    res.json({
      success: true,
      data: company
    });

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
    const existing = getCompanyByName(companyData.name);
    if (existing) {
      console.error('Company already exists, we return it:', companyData.name);
      return res.status(200).json({
        success: true,
        message: 'Company already exists',
        data: existing
      })
    }
    
    const newCompany = createCompany(companyData);
    
    console.log('✅ Company créée avec succès:', JSON.stringify(newCompany, null, 2));

    res.status(201).json({
      success: true,
      message: 'Company created successfully',
      data: newCompany
    });
  } catch (error) {
    console.error('❌ Erreur lors de la création de company:', error.message);
    console.error('📋 Stack trace:', error.stack);
    
    // Gestion des erreurs de validation
    if (error.message.includes('required') || error.message.includes('Invalid') || error.message.includes('already exists')) {
      console.error('❌ BadRequest détecté:', error.message);
      return res.status(400).json({
        success: false,
        error: 'Bad Request',
        message: error.message
      });
    }

    console.error('❌ Internal Server Error:', error.message);
    res.status(500).json({
      success: false,
      error: 'Internal server error',
      message: error.message
    });
  }
});

module.exports = router; 