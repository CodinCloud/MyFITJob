# TD : CI/CD avec GitHub Actions et Clever Cloud

## 🎯 Objectif du TD

Déployer automatiquement une application React sur Clever Cloud via un pipeline CI/CD avec GitHub Actions.

## 📋 Prérequis

- Compte GitHub
- Compte Clever Cloud (gratuit)
- Git installé sur votre machine

## 🚀 Étapes du TD

### 1. Fork du projet

1. Allez sur le repository GitHub du projet
2. Cliquez sur "Fork" en haut à droite
3. Clonez votre fork en local :
   ```bash
   git clone https://github.com/VOTRE_USERNAME/MyFITJob.git
   cd MyFITJob
   ```

### 2. Configuration Clever Cloud

1. **Créer un compte Clever Cloud**
   - Allez sur [console.clever-cloud.com](https://console.clever-cloud.com/)
   - Créez un compte gratuit

2. **Créer une application**
   - Dans la console Clever Cloud, cliquez sur "Create an application"
   - Choisissez "Node.js" comme type
   - Nommez votre application : `myfitjob-frontend`
   - Notez l'ID de l'application (visible dans les informations)

3. **Récupérer les tokens d'API**
   - Allez dans votre profil Clever Cloud
   - Section "API Keys"
   - Créez un nouveau token
   - Notez le `TOKEN` et le `SECRET`

### 3. Configuration des secrets GitHub

1. Dans votre repository GitHub, allez dans **Settings** > **Secrets and variables** > **Actions**
2. Ajoutez les secrets suivants :
   - `CLEVER_TOKEN` : Votre token Clever Cloud
   - `CLEVER_SECRET` : Votre secret Clever Cloud
   - `CC_APP_ID` : L'ID de votre application Clever Cloud

### 4. Personnalisation de l'application

1. **Modifier le header**
   - Ouvrez le fichier `src/MyFITJob.Frontend/src/components/Header.tsx`
   - Remplacez "TODO" par votre nom ou le nom de votre groupe
   - Committez vos changements :
   ```bash
   git add .
   git commit -m "feat: personnalisation du header avec mon nom"
   git push origin main
   ```

### 5. Test du pipeline CI/CD

1. **Créer une Pull Request**
   - Créez une nouvelle branche :
   ```bash
   git checkout -b feature/personnalisation
   ```
   - Faites une modification (par exemple, changez la couleur d'un élément)
   - Committez et poussez :
   ```bash
   git add .
   git commit -m "feat: modification pour tester le pipeline"
   git push origin feature/personnalisation
   ```
   - Créez une Pull Request sur GitHub

2. **Vérifier le déploiement**
   - Le pipeline GitHub Actions se déclenche automatiquement
   - Vérifiez que le build passe (icône verte)
   - L'URL de déploiement apparaîtra en commentaire de la PR

### 6. Vérification finale

1. **Accéder à l'application**
   - Cliquez sur l'URL fournie dans les commentaires de la PR
   - Vérifiez que l'application s'affiche correctement
   - Vérifiez que votre nom apparaît dans le header

2. **Tester les fonctionnalités**
   - Navigation dans l'application
   - Affichage des job offers (données mockées)
   - Graphiques d'analyse de marché

## ✅ Critères de réussite

- [ ] Le pipeline GitHub Actions passe (build vert)
- [ ] L'application est accessible via l'URL Clever Cloud
- [ ] Votre nom apparaît dans le header
- [ ] Les fonctionnalités (job offers, graphiques) fonctionnent
- [ ] Les données mockées s'affichent correctement

## 🔧 Dépannage

### Problème : Build échoue
- Vérifiez que tous les secrets sont configurés
- Vérifiez la syntaxe du code TypeScript

### Problème : Déploiement échoue
- Vérifiez les tokens Clever Cloud
- Vérifiez l'ID de l'application

### Problème : Application ne s'affiche pas
- Vérifiez les logs dans la console Clever Cloud
- Vérifiez que le fichier `.clever.json` est présent

## 📚 Ressources

- [Documentation Clever Cloud](https://www.clever-cloud.com/developers/doc/quickstart/)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [MSW Documentation](https://mswjs.io/)

## 🎉 Félicitations !

Vous avez réussi à mettre en place un pipeline CI/CD complet avec :
- Build automatique avec GitHub Actions
- Déploiement automatique sur Clever Cloud
- Tests et validation automatiques
- Personnalisation de l'application 