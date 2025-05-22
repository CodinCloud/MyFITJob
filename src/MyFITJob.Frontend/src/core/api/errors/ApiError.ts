export class ApiError extends Error {
  constructor(
    public code: number,
    public title: string,
    message: string,
    public endpoint?: string
  ) {
    super(message);
    this.name = 'ApiError';
  }

  static fromResponse(response: Response): ApiError {
    return new ApiError(
      response.status,
      this.getDefaultTitle(response.status),
      response.statusText || this.getDefaultMessage(response.status),
      response.url
    );
  }

  private static getDefaultTitle(status: number): string {
    switch (status) {
      case 401: return 'Non authentifié';
      case 403: return 'Accès refusé';
      case 404: return 'Non trouvé';
      case 500: return 'Erreur serveur';
      default: return 'Erreur';
    }
  }

  private static getDefaultMessage(status: number): string {
    switch (status) {
      case 401: return 'Vous devez être connecté pour accéder à cette ressource';
      case 403: return 'Vous n\'avez pas les droits nécessaires';
      case 404: return 'La ressource demandée n\'existe pas';
      case 500: return 'Une erreur est survenue sur le serveur';
      default: return 'Une erreur inattendue est survenue';
    }
  }

  toString(): string {
    return `${this.code} - ${this.title}: ${this.message} at ${this.endpoint}`;
  }
} 