<?php
// 允许上传的图片后缀
/* $allowedExts = array("gif", "jpeg", "jpg", "png","txt");
$temp = explode(".", $_FILES["file"]["name"]);
echo $_FILES["file"]["size"];
$extension = end($temp);     // 获取文件后缀名
if ((($_FILES["file"]["type"] == "image/gif")
|| ($_FILES["file"]["type"] == "image/jpeg")
|| ($_FILES["file"]["type"] == "image/jpg")
|| ($_FILES["file"]["type"] == "image/pjpeg")
|| ($_FILES["file"]["type"] == "image/x-png")
|| ($_FILES["file"]["type"] == "image/png"))
|| ($_FILES["file"]["type"] == "text/plain"))
&& ($_FILES["file"]["size"] < 2004800)   // 小于 200 kb
&& in_array($extension, $allowedExts))
{ */
    if ($_FILES["file"]["error"] > 0)
    {
        echo "错误：: " . $_FILES["file"]["error"] . "<br>";
    }
    else
    {
        echo "上传文件名: " . $_FILES["file"]["name"] . "<br>";
        echo "文件类型: " . $_FILES["file"]["type"] . "<br>";
        echo "文件大小: " . ($_FILES["file"]["size"] / 1024) . " kB<br>";
        echo "文件临时存储的位置: " . $_FILES["file"]["tmp_name"] . "<br>";
        
		
		//移动上传文件到uploads目录
 if(file_exists($newname ))
   {echo "<br/>您上传文件的已经在本地保存过";}
 else
   {
   	//文件以时间戳 重命名     
    date_default_timezone_set('PRC');//中国时区
    $D=date("YmdHis");//时间戳
    $filetype=substr(strrchr($_FILES['file']['name'],"."),1);//获取文件后缀名
    $newname=$D.".".$filetype; //文件新名字
    //echo "<br/>新文件名：".$newname;
 
     move_uploaded_file($_FILES['file']['tmp_name'], "upload/".$newname);//移动文件到指定文件夹uploads；
     echo "<br/>上传文件本地保存路径:"."./upload/".$newname;
         
   }
		
        /* // 判断当期目录下的 upload 目录是否存在该文件
        // 如果没有 upload 目录，你需要创建它，upload 目录权限为 777
        if (file_exists("upload/" . $_FILES["file"]["name"]))
        {
            echo $_FILES["file"]["name"] . " 文件已经存在。 ";
        }
        else
        {
            // 如果 upload 目录不存在该文件则将文件上传到 upload 目录下
            move_uploaded_file($_FILES["file"]["tmp_name"], "upload/" . $_FILES["file"]["name"]);
            echo "文件存储在: " . "upload/" . $_FILES["file"]["name"];
        } */
    }
/* }
else
{
    echo "非法的文件格式";
} */
?>