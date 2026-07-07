# Build the static bundle, then serve it with nginx (which also proxies /api).
FROM node:20-alpine AS build
WORKDIR /app
COPY package.json package-lock.json* ./
RUN npm install
COPY . .
# Empty VITE_API_URL -> relative /api paths, proxied by nginx to the API service.
ENV VITE_API_URL=""
RUN npm run build

FROM nginx:alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
