1. 解压 FileChecker.zip

2. 导出 Doc_DocList 的 ListDocPath 为 txt 文件并命名为 fileList.txt，可在配置文件修改此名称

3. 修改配置文件 FileChecker.exe.config ,主要是配置站点文件根路径和行列信息

4. 运行程序 FileChecker.exe

5. 查看生成的 
	
	out.txt （0为缺失，1为存在）
	found.txt（存在的文件列表）
	notfount.txt (缺失的文件列表) 
文件