# Thông tin cơ bản về database  

 # Công cụ hỗ trợ database
- SQLServer
- Docker 
 
 # Cách chạy database 
- Tải Docker desktop (nếu chạy commandline thì tải trên terminal)
- Chạy container: Docker desktop -> chạy container hoặc command line ```docker start sqlcontainer```

# Kết nối database
## 1, SQL Server

- Mở Sqlserver manangement studio
- Nhập thông tin kết nối  
Server type : Database Engine  
Server name : localhost, 1433  
Authentication: SQL Server Authentication  
Login: username (nằm ở file khác)
Password: password (nằm ở file khác)

## 2, PostgreSQL vscode extension

- Mở vscode, chọn database
- Nhập thông tin kết nối gồm Host, Port, Username, Password, Database (các thông tin này nằm ở file khác để tránh bị lộ)
# Cập nhật database
- Chạy migration: ```knex migrate:latest``` 

# Tham khảo 
https://medium.com/agilix/docker-express-running-a-local-sql-server-express-204890cff699
https://tdquang7.notion.site/T-o-migration-cho-database-12d8139672a080cabe27d29f5da82c7f?pvs=4