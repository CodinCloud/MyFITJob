import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { jobOffersApi } from './jobOffersApi';

/**
 * Hooks et fonctions React Query pour la gestion des offres d'emploi
 */
export const jobOfferKeys = {
  all: ['jobOffers'] as const,
  byStatus: (status: string) => [...jobOfferKeys.all, 'byStatus', status] as const,
};

export const useJobOffers = () => {
  const { data: jobOffersResult, isLoading: jobOffersLoading, error: jobOffersError } = useQuery({
    queryKey: jobOfferKeys.all,
    queryFn: jobOffersApi.fetchJobOffers,
    staleTime: 5 * 60 * 1000, // 5 minutes
    refetchOnWindowFocus: true,
    retry: false,
    throwOnError: false,
  });

  const jobOffers = jobOffersResult?.isSuccess ? jobOffersResult.value : [];

  return {
    data: jobOffersResult?.isSuccess ? 
      { isSuccess: true, value: jobOffers } : 
      jobOffersResult,
    isLoading: jobOffersLoading,
    error: jobOffersError 
  };
};

export const useCreateJobOffer = () => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: jobOffersApi.createJobOffer,
    onSuccess: () => {
      // Invalider le cache pour rafraîchir la liste
      queryClient.invalidateQueries({ queryKey: jobOfferKeys.all });
    },
  });
}; 