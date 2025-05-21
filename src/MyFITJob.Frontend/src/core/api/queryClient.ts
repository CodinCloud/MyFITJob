import { QueryClient } from '@tanstack/react-query';

// Créer un client de requête avec gestionnaire d'erreurs global
export const createQueryClient = () => {
  return new QueryClient({
    defaultOptions: {
      queries: {
        retry: 1,
        refetchOnWindowFocus: false,
      },
      mutations: {
        // Options spécifiques aux mutations si nécessaire
      }
    }
  });
}; 