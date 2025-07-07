# MyFITJob - TD CI/CD avec Clever Cloud

## 🎯 Objectif du TD

Déployer automatiquement une application React sur Clever Cloud via un pipeline CI/CD avec GitHub Actions.

## 📁 Structure du projet

```
MyFITJob/
├── src/
│   └── MyFITJob.Frontend/     # Application React + Vite
├── .github/
│   └── workflows/
│       └── ci_cd_clever.yml   # Pipeline GitHub Actions
├── docs/
│   └── TD_frontend_clever.md  # Guide du TD
├── .clever.json               # Configuration Clever Cloud
└── package.json               # Scripts de build
```

## 🚀 Getting Started

### Prérequis

- Node.js 18+
- Git
- Compte GitHub
- Compte Clever Cloud (gratuit) à créer avec votre adresse `@3il.fr` 

## 📚 Documentation

- **Guide complet du TD** : [docs/TD_frontend_clever.md](docs/TD_frontend_clever.md)
- **Configuration Clever Cloud** : Voir la documentation officielle

## 🎓 Objectifs pédagogiques

- Comprendre les concepts de CI/CD
- Mettre en place un pipeline GitHub Actions
- Déployer automatiquement sur un cloud provider
- Utiliser des outils de mock pour le développement

## 🔧 Fonctionnalités

- **Interface moderne** : React avec ShadCN/UI
- **Données mockées** : API simulée avec MSW
- **Kanban des offres** : Gestion visuelle des jobs
- **Analyse de marché** : Graphiques et statistiques
- **Header personnalisable** : Avatar et nom modifiables

## 📋 Étapes du TD

1. **Fork du projet** sur GitHub
2. **Configuration Clever Cloud** (compte + application)
3. **Configuration des secrets** GitHub Actions
4. **Personnalisation** du header avec votre nom
5. **Test du pipeline** via Pull Request
6. **Vérification** du déploiement

## 🎉 Résultat attendu

Une application web déployée automatiquement sur Clever Cloud avec :
- ✅ Build automatique via GitHub Actions
- ✅ Déploiement automatique sur Clever Cloud
- ✅ Interface personnalisée avec votre nom
- ✅ Fonctionnalités complètes (Kanban, graphiques)
- ✅ Données mockées fonctionnelles

## 📖 Ressources

- [Documentation Clever Cloud](https://www.clever-cloud.com/developers/doc/quickstart/)
- [GitHub Actions](https://docs.github.com/en/actions)
- [MSW Documentation](https://mswjs.io/)
- [ShadCN/UI](https://ui.shadcn.com/)




