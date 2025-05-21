import { useQuery } from '@tanstack/react-query';
import type { MostSoughtSkill } from '../marketAnalysisTypes';

export const marketAnalysisApi = {
  fetchMostSoughtSkills: async (): Promise<MostSoughtSkill[]> => {
    const res = await fetch('/api/analytics/skills');
    if (!res.ok) throw new Error('Erreur lors du chargement des skills');
    return res.json();
  },
};

export const marketAnalysisKeys = {
  all: ['marketAnalysis'] as const,
  mostSoughtSkills: () => [...marketAnalysisKeys.all, 'mostSoughtSkills'] as const,
};

export const useMostSoughtSkills = () => {
  return useQuery({
    queryKey: marketAnalysisKeys.mostSoughtSkills(),
    queryFn: marketAnalysisApi.fetchMostSoughtSkills,
    staleTime: 5 * 60 * 1000, 
    refetchOnWindowFocus: true,
    retry: false,
    throwOnError: false,
  });
}; 