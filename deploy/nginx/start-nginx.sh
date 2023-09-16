envsubst '$DOMAIN_NAME' < /etc/nginx/conf.d/nginx.template > /etc/nginx/conf.d/default.conf
nginx -g 'daemon off;'
