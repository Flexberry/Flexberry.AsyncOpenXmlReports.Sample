docker build --no-cache -f SQL\Dockerfile.PostgreSql -t asyncopenxmlreportssample/postgre-sql ../SQL

docker build --no-cache -f Dockerfile -t asyncopenxmlreportssample/app ../..
