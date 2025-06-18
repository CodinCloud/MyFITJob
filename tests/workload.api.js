import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate, Trend } from 'k6/metrics';

// Métriques personnalisées
const errorRate = new Rate('errors');
const statsTrend = new Trend('stats_duration');
const jobOffersTrend = new Trend('joboffers_duration');

// Configuration du test
export const options = {
  stages: [
    { duration: '30s', target: 10 },    
    { duration: '1m', target: 100 },     
    // { duration: '30s', target: 1000 },   // Montée à 1000 utilisateurs
    // { duration: '1m', target: 1000 },    // Maintien à 1000 utilisateurs
    { duration: '30s', target: 0 },      // Ramp-down
  ],
  thresholds: {
    'errors': ['rate<0.1'],              // Moins de 10% d'erreurs
    'http_req_duration': ['p(95)<2000'], // 95% des requêtes sous 2s
  },
};

// Headers pour simuler un navigateur Chrome
const headers = {
  'Accept': 'application/json',
  'Content-Type': 'application/json',
  'Cache-Control': 'max-age=0',
  'User-Agent': 'k6-loadtest'
};

// Fonction principale
export default function () {
 
  // Endpoint skills stats - Memory Intensive
  // const statsRes = http.get('http://localhost:8081/api/market/skills', { headers, tags: { name: 'skills' } });
  // check(statsRes, { 'skills 200': (r) => r.status === 200 });
  // statsTrend.add(statsRes.timings.duration);
  // errorRate.add(statsRes.status !== 200);

  // Endpoint job offers
  const jobOffersRes = http.get('http://localhost:8081/api/joboffers', { headers, tags: { name: 'joboffers' } });
  check(jobOffersRes, { 'joboffers 200': (r) => r.status === 200 });
  jobOffersTrend.add(jobOffersRes.timings.duration);
  errorRate.add(jobOffersRes.status !== 200);

  sleep(1);
}
