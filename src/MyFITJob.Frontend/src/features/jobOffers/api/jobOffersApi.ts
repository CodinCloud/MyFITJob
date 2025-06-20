import type { JobOffer } from '../jobOffersTypes';
import { Result } from '@/core/functional/Result';

/**
 * API pour la gestion des offres d'emploi (pattern Result<T>)
 */
export const jobOffersApi = {
  fetchJobOffers: async (): Promise<Result<JobOffer[]>> => {
    try {
      const response = await fetch('/api/jobOffers');
      if (!response.ok) {
        return Result.failure(new Error('Erreur lors du chargement des offres d\'emploi'));
      }
      const data = await response.json();
      return Result.success(data);
    } catch (error) {
      return Result.failure(error instanceof Error ? error : new Error('Erreur inconnue'));
    }
  },
}; 