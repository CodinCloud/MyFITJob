# --- Étape 1 : Build de l'application React ---
FROM node:20-alpine AS build
WORKDIR /app

# Copie les fichiers package.json et package-lock.json du frontend
COPY src/MyFITJob.Frontend/package*.json ./

# Copie tout le code source du frontend
COPY src/MyFITJob.Frontend/ ./

# Installation des dépendances et build de l'app
RUN npm ci
RUN npm run build

# --- Étape 2 : Nginx pour servir le build ---
FROM nginx:stable-alpine AS prod

# Copie le build dans le dossier Nginx
COPY --from=build /app/dist /usr/share/nginx/html

# Copie la config Nginx personnalisée
COPY src/MyFITJob.Frontend/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 8080

CMD ["nginx", "-g", "daemon off;"]

# ---
# Instructions :
# - Le build et le run se font sur le dossier src/MyFITJob.Frontend
# - Le build Clever Cloud fonctionnera sans .clever.json ni variable spéciale
# - Pour tester en local :
#   docker build -t myfitjob .
#   docker run -p 8080:8080 myfitjob 