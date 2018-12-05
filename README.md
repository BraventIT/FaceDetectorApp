# FaceDetectorDemo
Ejemplo de Face API integrado en una aplicación de Xamarin.Forms

Para que la aplicación compile, es necesario dar de alta un servicio en Azure de Face API y otro de Blob Storage. En siguientes enlaces se describe cómo hacerlo:


Servicio Face API: https://docs.microsoft.com/es-es/azure/cognitive-services/cognitive-services-apis-create-account

Servicio Blob Storage: https://blog.xamarin.com/xamarin-plus-azure-blob-cloud-storage/

A continuación, en la clase FaceApiConstants.cs del proyecto, será necesario asignar a la constante BASE_ENDPOINT_FACE_API el endpoint del servicio de Azure de FaceApi, así como a FACE_API_SUSCRIPTION_KEY_VALUE la clave de suscripción del mismo.

Por otro lado, en la clase BlobStorageConstants.cs, habrá que asignar también las constantes BLOB_STORAGE_CONTAINER_NAME y BLOB_STORAGE_ENDPOINT, por el nombre y el endpoint del blob storage que hayamos dado de alta en Azure.

¡Ya tienes tu aplicación preparada para empezar a identificar personas! 

Notas: Por defecto al arrancar la aplicación se crea un grupo de personas llamado Grupo1. Si quieres cambiar este nombre puedes hacerlo en FaceApiConstants.cs. Así mismo para la posterior identificación de caras similares dentro de un listado, también se crea un LargeList llamado Grupo1LargeList. Puedes cambiar su nombre en la misma clase.

Para más información de FaceAPI: https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236