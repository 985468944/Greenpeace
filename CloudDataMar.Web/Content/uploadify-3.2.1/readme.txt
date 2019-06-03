1:关于cancelImg属性属性的问题。
我们使用的uploadify-3.2.1版本中已经没有cancelImg属性。
cancelImg在uploadify.css中定义，默认为background: url('../img/uploadify-cancel.png') 0 0 no-repeat;
我们在css文件中按需手动调整uploadify-cancel.png图片文位置

2:关于上传Action命名和上传文件存放目录命名冲突的原因
案例：上传Action我们定义为UploadController	文件上传存放目录我们定义为~/Upload/yyyyMMdd 此时存在冲突请注意避免
我们文件存放目录采用~/Files/Upload/

		 