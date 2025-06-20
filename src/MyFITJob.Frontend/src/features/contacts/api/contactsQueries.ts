import { useQuery } from '@tanstack/react-query';
import { contactsApi } from './contactsApi';
import type { CompanyInfo } from '../contactsTypes';

/**
 * Hooks et fonctions React Query pour la gestion des contacts
 */
export const contactsKeys = {
  all: ['contacts'] as const,
  companyInfo: (companyId: number) => [...contactsKeys.all, 'companyInfo', companyId] as const,
  companyInfoBatch: (companyIds: number[]) => [...contactsKeys.all, 'companyInfoBatch', companyIds] as const,
};

export const useCompanyInfo = (companyId: number) => {
  return useQuery({
    queryKey: contactsKeys.companyInfo(companyId),
    queryFn: () => contactsApi.fetchCompanyInfo(companyId),
    staleTime: 10 * 60 * 1000, // 10 minutes
    refetchOnWindowFocus: false,
    retry: 2,
    throwOnError: false,
    enabled: !!companyId,
  });
};

export const useCompanyInfoBatch = (companyIds: number[]) => {
  return useQuery({
    queryKey: contactsKeys.companyInfoBatch(companyIds),
    queryFn: () => contactsApi.fetchCompanyInfoBatch(companyIds),
    staleTime: 10 * 60 * 1000, // 10 minutes
    refetchOnWindowFocus: false,
    retry: 1,
    throwOnError: false,
    enabled: companyIds.length > 0,
  });
};

/**
 * Hook utilitaire pour enrichir les offres d'emploi avec les informations d'entreprise
 */
export const useEnrichedJobOffers = (jobOffers: any[]) => {
  const companyIds = jobOffers.map(job => job.companyId || job.id).filter(Boolean);
  
  const { data: companyInfoMap, isLoading, error } = useCompanyInfoBatch(companyIds);

  const unavailableCompanyInfo = {
    industry: "indisponible",
    size: "indisponible",
    rating: "indisponible"
  };

  const enrichedJobOffers = jobOffers.map(job => ({
    ...job,
    companyInfo:
      companyInfoMap?.isSuccess
        ? companyInfoMap.value[job.companyId || job.id]
        : unavailableCompanyInfo
  }));

  return {
    enrichedJobOffers,
    isLoading,
    error: error || (companyInfoMap?.isFailure ? companyInfoMap.error : undefined)
  };
}; 