docker build --no-cache -f SQL\Dockerfile.PostgreSql -t asyncopenxmlreportssample/postgre-sql ../SQL

docker build --no-cache -f Dockerfile.Keycloak -t asyncopenxmlreportssample/keycloak ../..

docker build --no-cache -f Dockerfile -t asyncopenxmlreportssample/app ../..

docker build --no-cache -f Dockerfile.Quartz -t asyncopenxmlreportssample/quartz ../..

docker build --no-cache -f Nginx\Dockerfile.Nginx -t asyncopenxmlreportssample/nginx Nginx