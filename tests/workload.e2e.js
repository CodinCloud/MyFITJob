import http from 'k6/http';
import { check } from 'k6';
import { Rate, Trend } from 'k6/metrics';
import { browser } from 'k6/browser';

// Métriques personnalisées
const errorRate = new Rate('errors');
const homepageTrend = new Trend('homepage_duration');
const loadingTimeTrend = new Trend('loading_text_duration');

// Configuration du test de montée en charge
export const options = {
  scenarios: {
    // Scénario 1: Tests browser (limité pour les ressources)
    ui_browser: {
      executor: 'constant-vus',
      vus: 2,           // Seulement 2 navigateurs simultanés
      duration: '3m',   // Pendant toute la durée du test
      options: {
        browser: {
          type: 'chromium',
        },
      },
    },
    
    // Scénario 2: Tests HTTP pour la montée en charge massive
    http_load: {
      executor: 'ramping-vus',
      startVUs: 0,
      stages: [
        { duration: '30s', target: 50 },    // Montée rapide à 50 req/s
        { duration: '1m', target: 200 },    // Montée à 200 req/s  
        { duration: '1m', target: 500 },    // Montée à 500 req/s
        { duration: '30s', target: 50 },    // Redescente à 50 req/s
      ],
      exec: 'httpLoadTest',  // Fonction séparée pour HTTP
    },
  },
  
  thresholds: {
    'errors': ['rate<0.1'],                    // Moins de 10% d'erreurs
    'http_req_duration': ['p(95)<3000'],       // 95% des requêtes sous 3s
    'loading_text_duration': ['p(95)<5000'],   // 95% du temps de chargement sous 5s
    'http_req_duration{name:homepage}': ['p(95)<2000'],
    'http_req_duration{name:api}': ['p(95)<1000'],
  },
};

const headers = {
  'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36',
  'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8',
  'Accept-Language': 'fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7',
  'Accept-Encoding': 'gzip, deflate, br',
  'Connection': 'keep-alive',
  'Upgrade-Insecure-Requests': '1',
  'Sec-Fetch-Dest': 'document',
  'Sec-Fetch-Mode': 'navigate',
  'Sec-Fetch-Site': 'none',
  'Sec-Fetch-User': '?1',
  'Cache-Control': 'max-age=0'
};

// Fonction pour mesurer le temps d'affichage du texte "Chargement..."
async function measureLoadingTextDuration(page) {
  const loadingStartTime = Date.now();
  
  try {
    // Liste des sélecteurs possibles pour le texte "Chargement..."
    const loadingSelectors = [
      'text=Chargement...',
      'div:has-text("Chargement...")',
      '[class*="loading"] >> text=Chargement...',
      'div >> text=/Chargement/'
    ];
    
    // Attendre qu'au moins un des sélecteurs de chargement apparaisse
    let loadingDetected = false;
    for (const selector of loadingSelectors) {
      try {
        await page.waitForSelector(selector, { timeout: 2000 });
        console.log(`Texte "Chargement..." détecté avec le sélecteur: ${selector}`);
        loadingDetected = true;
        break;
      } catch (e) {
        // Continuer avec le sélecteur suivant
        continue;
      }
    }
    
    if (!loadingDetected) {
      console.log('Aucun texte "Chargement..." détecté');
      return -1;
    }
    
    // Attendre que tous les textes "Chargement..." disparaissent
    // On attend que la page soit complètement chargée
    await page.waitForLoadState('networkidle', { timeout: 10000 });
    
    // Vérifier que le texte "Chargement..." a disparu
    const remainingLoadingElements = await page.locator('text=Chargement...').count();
    if (remainingLoadingElements > 0) {
      // Attendre un peu plus si des éléments de chargement sont encore présents
      await page.waitForTimeout(1000);
    }
    
    const loadingEndTime = Date.now();
    const loadingDuration = loadingEndTime - loadingStartTime;
    
    console.log(`Durée d'affichage du texte "Chargement...": ${loadingDuration}ms`);
    
    // Enregistrer la métrique
    loadingTimeTrend.add(loadingDuration);
    
    return loadingDuration;
  } catch (error) {
    console.log('Erreur lors de la mesure du temps de chargement:', error.message);
    return -1;
  }
}

// Fonction principale pour les tests browser (mesure du "Chargement...")
export default async function () {
  const context = await browser.newContext();
  const page = await context.newPage();
  
  try {
    // Naviguer vers la page d'accueil
    await page.goto('http://localhost:3000/');
    
    // Mesurer le temps d'affichage du texte "Chargement..."
    const loadingDuration = await measureLoadingTextDuration(page);
    
    // Vérifications supplémentaires pour s'assurer que les composants sont chargés
    let kanbanLoaded = false;
    let chartsLoaded = false;
    
    try {
      await page.waitForSelector('[class*="kanban"], [class*="grid"]', { timeout: 5000 });
      kanbanLoaded = true;
    } catch {
      kanbanLoaded = false;
    }
    
    try {
      await page.waitForSelector('[class*="chart"], svg', { timeout: 5000 });
      chartsLoaded = true;
    } catch {
      chartsLoaded = false;
    }
    
    // Checks synchrones
    check(null, {
      'kanban_loaded': () => kanbanLoaded,
      'charts_loaded': () => chartsLoaded,
      'loading_text_measured': () => loadingDuration > 0,
      'loading_duration_reasonable': () => loadingDuration < 10000 || loadingDuration === -1
    });
    
  } finally {
    await page.close();
  }
}

// Fonction pour les tests HTTP de montée en charge
export function httpLoadTest() {
  // Test du Frontend
  const homeRes = http.get('http://localhost:3000/', {
    headers: headers,
    tags: { name: 'homepage' }
  });

  // Test des assets statiques (pour voir la charge sur Nginx/serveur web)
  const jsRes = http.get('http://localhost:3000/assets/index-CBmt8EXl.js', {
    headers: headers,
    tags: { name: 'static_js' }
  });

  const cssRes = http.get('http://localhost:3000/assets/index-BrnsacLJ.css', {
    headers: headers,
    tags: { name: 'static_css' }
  });

  // Test de l'API (charge sur Backend + BDD)
  const apiJobsRes = http.get('http://localhost:8081/api/joboffers', {
    headers: { 'Accept': 'application/json' },
    tags: { name: 'api' }
  });

  // Test API gourmande (pour tester la BDD sous charge)
  const apiSkillsRes = http.get('http://localhost:8081/api/market/skills?top=10', {
    headers: { 'Accept': 'application/json' },
    tags: { name: 'api_heavy' }
  });

  // Vérifications de performance
  check(homeRes, {
    'frontend_status_200': (r) => r.status === 200,
    'frontend_fast': (r) => r.timings.duration < 2000,
  });

  check(apiJobsRes, {
    'api_status_200': (r) => r.status === 200,
    'api_fast': (r) => r.timings.duration < 1000,
  });

  check(apiSkillsRes, {
    'api_heavy_status_200': (r) => r.status === 200,
    'api_heavy_acceptable': (r) => r.timings.duration < 3000,
  });

  // Enregistrement des métriques
  errorRate.add(homeRes.status !== 200);
  errorRate.add(apiJobsRes.status !== 200);
  errorRate.add(apiSkillsRes.status !== 200);
  
  homepageTrend.add(homeRes.timings.duration);
}
