import type { CompanyInfo } from '../contactsTypes';
import { Result } from '@/core/functional/Result';

const CONTACTS_SERVICE_URL = 'http://localhost:3010';

/**
 * API pour la gestion des contacts et informations d'entreprises (pattern Result<T>)
 */
export const contactsApi = {
  fetchCompanyInfo: async (companyId: number): Promise<Result<CompanyInfo>> => {
    try {
      const response = await fetch(`${CONTACTS_SERVICE_URL}/api/companies/${companyId}`);
      if (!response.ok) {
        return Result.failure(new Error(`Erreur lors du chargement des informations de l'entreprise ${companyId}`));
      }
      const data = await response.json();
      return Result.success(data);
    } catch (error) {
      return Result.failure(error instanceof Error ? error : new Error('Erreur inconnue lors de la récupération des informations d\'entreprise'));
    }
  },

  fetchCompanyInfoBatch: async (companyIds: number[]): Promise<Result<Record<number, CompanyInfo>>> => {
    try {
      const promises = companyIds.map(id => 
        fetch(`${CONTACTS_SERVICE_URL}/api/companies/${id}`)
          .then(response => response.ok ? response.json() : null)
          .catch(() => null)
      );

      const results = await Promise.all(promises);
      const companyInfoMap: Record<number, CompanyInfo> = {};

      companyIds.forEach((id, index) => {
        if (results[index]) {
          companyInfoMap[id] = results[index];
        }
      });

      // Si aucune info n'a pu être récupérée, c'est un échec global
      if (Object.keys(companyInfoMap).length === 0) {
        return Result.failure(new Error("Service contacts indisponible ou aucune entreprise trouvée"));
      }

      return Result.success(companyInfoMap);
    } catch (error) {
      return Result.failure(error instanceof Error ? error : new Error('Erreur lors du chargement en lot des informations d\'entreprises'));
    }
  },
}; 