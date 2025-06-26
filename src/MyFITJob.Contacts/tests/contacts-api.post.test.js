const axios = require('axios');

const CONTACTS_API_BASE_URL = 'http://localhost:3002';

// Configuration axios pour les tests
const api = axios.create({
  baseURL: CONTACTS_API_BASE_URL,
  timeout: 5000,
  headers: {
    'Content-Type': 'application/json'
  }
});

async function testCreateCompany() {
  console.log('🧪 Test POST /api/companies');
  
  try {
    const response = await axios.post('http://localhost:3002/api/companies', {
      name: 'Test Company',
      industry: 'Tech',
      size: '51-200',
      description: 'Une entreprise de test'
    });

    console.log('✅ Succès - Company créée');
    console.log(`   ID: ${response.data.data.id}`);
    console.log(`   Nom: ${response.data.data.name}`);
    
  } catch (error) {
    console.log('❌ Erreur:', error.response?.data?.message || error.message);
  }
}

// Test de vérification de la connectivité
async function testConnectivity() {
  try {
    console.log('\n🔍 Test de connectivité...');
    const response = await api.get('/health');
    if (response.status === 200) {
      console.log('✅ Service Contacts accessible');
      return true;
    }
  } catch (error) {
    console.log('❌ Service Contacts inaccessible');
    console.log(`   Erreur: ${error.message}`);
    return false;
  }
}

// Fonction principale
async function runTests() {
  console.log('🚀 Démarrage des tests d\'intégration - API Contacts');
  
  const isConnected = await testConnectivity();
  if (!isConnected) {
    console.log('\n❌ Impossible de continuer les tests - Service inaccessible');
    process.exit(1);
  }
  
  await testCreateCompany();
  
  console.log('\n🏁 Tests terminés');
}

// Exécution des tests
if (require.main === module) {
  runTests().catch(error => {
    console.log(`\n💥 Erreur lors de l'exécution des tests: ${error.message}`);
    process.exit(1);
  });
}

module.exports = { testCreateCompany, testConnectivity }; 