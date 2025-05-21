import { useQuery } from '@tanstack/react-query';
import { jobOffersApi } from './jobOffersApi';

/**
 * Hooks et fonctions React Query pour la gestion des offres d'emploi
 */
export const jobOfferKeys = {
  all: ['jobOffers'] as const,
  byStatus: (status: string) => [...jobOfferKeys.all, 'byStatus', status] as const,
};

export const useJobOffers = () => {
  return useQuery({
    queryKey: jobOfferKeys.all,
    queryFn: jobOffersApi.fetchJobOffers,
    staleTime: 5 * 60 * 1000, // 5 minutes
    refetchOnWindowFocus: true,
    retry: false,
    throwOnError: false,
  });
}; 