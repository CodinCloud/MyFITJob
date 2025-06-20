const express = require('express');
const router = express.Router();
const { getCompanyById } = require('../data/companies');

// GET /api/companies/:id
router.get('/:id', (req, res) => {
  const companyId = req.params.id;
  
  try {
    const company = getCompanyById(companyId);
    
    if (!company) {
      return res.status(404).json({
        error: 'Company not found',
        message: `No company found with ID ${companyId}`
      });
    }
    
    // Return only the companyInfo structure as expected by the frontend
    const companyInfo = {
      industry: company.industry,
      size: company.size,
      rating: company.rating
    };
    
    res.json(companyInfo);
    
  } catch (error) {
    console.error('Error fetching company:', error);
    res.status(500).json({
      error: 'Internal server error',
      message: 'Failed to fetch company information'
    });
  }
});

module.exports = router; 