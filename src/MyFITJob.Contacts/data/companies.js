// Base de données en mémoire pour les companies
let companies = [];

let nextId = 11; // Pour générer de nouveaux IDs

function getCompanyById(id) {
  const company = companies.find(c => c.id === parseInt(id));
  return company || null;
}

function getCompanyByName(name) {
  const company = companies.find(c => c.name === name);
  return company || null;
}

function getAllCompanies() {
  return companies;
}

function createCompany(companyData) {
  
  // Vérification des champs requis
  if (!companyData.name) {
    throw new Error('Name is required');
  }
  if (!companyData.industry) {
    throw new Error('Industry is required');
  }
  if (!companyData.size) {
    throw new Error('Size is required');
  }

  // Vérifier si le nom existe déjà
  const existingCompany = companies.find(c => c.name.toLowerCase() === companyData.name.toLowerCase());
  if (existingCompany) {
    throw new Error('A company with this name already exists');
  }

  const newCompany = {
    id: nextId++,
    name: companyData.name,
    industry: companyData.industry,
    size: companyData.size,
    rating: companyData.rating || parseFloat((Math.random() * 2 + 3).toFixed(1)), // Rating par défaut entre 3.0 et 5.0
    description: companyData.description || ''
  };

  companies.push(newCompany);
  return newCompany;
}

module.exports = {
  getCompanyById,
  getCompanyByName,
  getAllCompanies,
  createCompany,
}; 