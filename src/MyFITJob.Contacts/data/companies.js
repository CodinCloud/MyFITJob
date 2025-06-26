const fs = require('fs').promises;
const path = require('path');

// Base de données persistante pour les companies
let companies = [];
let nextId = 1;

// Fichier de données
const dataFile = path.join(process.env.DATA_DIR || '/app/data', 'companies.json');

// Initialise les données au démarrage
async function initializeData() {
  try {
    // Créer le dossier de données s'il n'existe pas
    const dataDir = path.dirname(dataFile);
    await fs.mkdir(dataDir, { recursive: true });
    
    // Essayer de charger les données existantes
    try {
      const data = await fs.readFile(dataFile, 'utf8');
      const parsed = JSON.parse(data);
      companies = parsed.companies || [];
      nextId = parsed.nextId || 1;
      console.log(`📁 Données chargées: ${companies.length} companies`);
    } catch (error) {
      if (error.code === 'ENOENT') {
        // Fichier n'existe pas, base vide
        await saveData();
        console.log('📁 Base de données initialisée (vide)');
      } else {
        throw error;
      }
    }
  } catch (error) {
    console.error('❌ Erreur lors de l\'initialisation:', error);
    throw error;
  }
}

// Sauvegarde les données
async function saveData() {
  try {
    const data = {
      companies: companies,
      nextId: nextId,
      lastUpdated: new Date().toISOString()
    };
    await fs.writeFile(dataFile, JSON.stringify(data, null, 2));
  } catch (error) {
    console.error('❌ Erreur lors de la sauvegarde:', error);
    throw new Error('Impossible de sauvegarder les données');
  }
}

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

async function createCompany(companyData) {
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
  
  // Sauvegarder immédiatement
  await saveData();
  
  return newCompany;
}

module.exports = {
  getCompanyById,
  getCompanyByName,
  getAllCompanies,
  createCompany,
  initializeData
}; 