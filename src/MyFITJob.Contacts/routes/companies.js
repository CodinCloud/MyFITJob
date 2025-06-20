const express = require('express');
const router = express.Router();
const { getCompanyById, getRandomCompanyInfo } = require('../data/companies');

// GET /api/companies/:id
router.get('/:id', (req, res) => {
  const companyId = req.params.id;
  let company = getCompanyById(companyId);

  // Si la company n'existe pas, on retourne quand même un objet aléatoire
  if (!company) {
    // On peut donner un nom générique ou "Unknown Company"
    company = {
      id: companyId,
      name: `Unknown Company #${companyId}`,
      ...getRandomCompanyInfo()
    };
  }

  const companyInfo = {
    industry: company.industry,
    size: company.size,
    rating: company.rating
  };

  res.json(companyInfo);
});

module.exports = router; 