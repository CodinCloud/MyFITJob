import { useQuery } from '@tanstack/react-query';
import type { MostSoughtSkill, Skill } from '../marketAnalysisTypes';

export const marketAnalysisApi = {
  fetchMostSoughtSkills: async (): Promise<MostSoughtSkill[]> => {
    const res = await fetch('/api/market/skills?top=5');
    if (!res.ok) throw new Error('Erreur lors du chargement des skills');
    const skills: Skill[] = await res.json();
    // Mapping pour le chart
    return skills.map(({ skill, count }) => ({ name: skill, count }));
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