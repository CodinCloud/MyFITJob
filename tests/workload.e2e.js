import http from 'k6/http';
import { check } from 'k6';
import { Rate, Trend } from 'k6/metrics';

// Métriques personnalisées
const errorRate = new Rate('errors');
const homepageTrend = new Trend('homepage_duration');

// Configuration du test
export const options = {
  stages: [
    { duration: '30s', target: 100 },    // Montée rapide à 100 utilisateurs
    { duration: '1m', target: 1000 },    // Maintien à 1000 utilisateurs
    { duration: '1m', target: 2000 },    // Maintien à 1000 utilisateurs
    { duration: '30s', target: 100 },      // Ramp-down
  ],

  thresholds: {
    'errors': ['rate<0.1'],              // Moins de 10% d'erreurs
    'http_req_duration': ['p(95)<2000'], // 95% des requêtes sous 2s
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

// Fonction principale
export default function () {
  // Visite de la page d'accueil avec les headers d'un navigateur
  const homeRes = http.get('http://localhost:3000/', {
    headers: headers,
    tags: { name: 'homepage' }
  });

  const js = http.get('http://localhost:3000/assets/index-CBmt8EXl.js', {
    headers: headers,
    tags: { name: 'js' }
  });

  const js2 = http.get('http://localhost:3000/assets/index-DDw97B-X.js', {
    headers: headers,
    tags: { name: 'js' }
  });
  
  const css = http.get('http://localhost:3000/assets/index-BrnsacLJ.css', {
    headers: headers,
    tags: { name: 'css' }
  });

  // Vérifications
  check(homeRes, {
    'status is 200': (r) => r.status === 200,
    'loads fast': (r) => r.timings.duration < 1000,
  });

  // Enregistrement des métriques
  errorRate.add(homeRes.status !== 200);
  homepageTrend.add(homeRes.timings.duration);
}
