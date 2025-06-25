import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { jobOffersApi } from './jobOffersApi';
import { useEnrichedJobOffers } from '@/features/contacts/api/contactsQueries';
import type { CreateJobOffer } from '../jobOffersTypes';

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
  
  const { enrichedJobOffers, isLoading: contactsLoading } = useEnrichedJobOffers(jobOffers);

  return {
    data: jobOffersResult?.isSuccess ? 
      { isSuccess: true, value: enrichedJobOffers } : 
      jobOffersResult,
    isLoading: jobOffersLoading || contactsLoading,
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