# Stage 1 : build Vite
FROM node:20-alpine AS base 
WORKDIR /app
COPY . .
RUN npm install

# Stage BUILD : compile en dist/
FROM base AS build
COPY . .
RUN npm run build

# Stage PROD : nginx minimal
FROM nginx:stable-alpine AS prod
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]


# Stage DEV : live-reload
FROM base AS dev
RUN npm install --save-dev vite
ENV NODE_ENV=development
CMD ["npm", "run", "dev", "--", "--host", "0.0.0.0"]
