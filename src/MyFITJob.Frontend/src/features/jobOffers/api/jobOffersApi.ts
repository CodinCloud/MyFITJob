import type { JobOffer, CreateJobOffer } from '../jobOffersTypes';
import { Result } from '@/core/functional/Result';

// const url = "http://localhost:8081" 
const url = ""

/**
 * API pour la gestion des offres d'emploi (pattern Result<T>)
 */
export const jobOffersApi = {
  fetchJobOffers: async (): Promise<Result<JobOffer[]>> => {
    try {
      const response = await fetch(`${url}/api/jobOffers`);
      if (!response.ok) {
        return Result.failure(new Error('Erreur lors du chargement des offres d\'emploi'));
      }
      const data = await response.json();
      return Result.success(data);
    } catch (error) {
      return Result.failure(error instanceof Error ? error : new Error('Erreur inconnue'));
    }
  },

  createJobOffer: async (jobOffer: CreateJobOffer): Promise<Result<JobOffer>> => {
    try {
      const response = await fetch(`${url}/api/jobOffers`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(jobOffer),
      });
      
      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        return Result.failure(new Error(errorData.message || 'Erreur lors de la création de l\'offre d\'emploi'));
      }
      
      const data = await response.json();
      return Result.success(data);
    } catch (error) {
      return Result.failure(error instanceof Error ? error : new Error('Erreur inconnue'));
    }
  },
}; 