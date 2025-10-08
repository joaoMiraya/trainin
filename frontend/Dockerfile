# Etapa 1: Construção da aplicação
FROM node:22 AS builder

# Define o diretório de trabalho dentro do container
WORKDIR /app

# Copia os arquivos do projeto para o container
COPY package.json package-lock.json ./
RUN npm install

# Copia o restante dos arquivos e compila a aplicação
COPY . .
RUN npm run build

# Etapa 2: Configuração do servidor Nginx
FROM nginx:1.23

# Remove configuração padrão do Nginx
RUN rm -rf /etc/nginx/conf.d

# Copia a configuração customizada do Nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copia os arquivos compilados para a pasta servida pelo Nginx
COPY --from=builder /app/dist /usr/share/nginx/html

# Expõe a porta 80 para o tráfego HTTP
EXPOSE 80

# Comando de inicialização do Nginx
CMD ["nginx", "-g", "daemon off;"]
