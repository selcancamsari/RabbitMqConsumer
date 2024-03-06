using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//Bağlantı Oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://wchpyyvv:xFLMWyo7qrUU2fBHE7wNgq1_L9nzrWUF@toad.rmq.cloudamqp.com/wchpyyvv");

//Bağlantıyı Aktifleştirme ve Kanal Açma
//using kullanıyoruz çünkü bellekte yer etmesin.
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Queue Oluşturma
//durable parametresi mesajların kuyrukta ne kadar kalacağını belirtir.
//exclusive parametresi kuyruğun özel olup olmadığı. Birden fazla kuyruk ile özel olarak işlem yapıp yapamayacağımızı belirtir.
//autoDelete tüm mesajların tüketildiğinde kuyruğun silinip silinemeyeceğine dair yapılanmadır.
//consumer'da da kuyruk publisherdaki ile birebir aynı yapılanmada tanımlanmalıdır.
channel.QueueDeclare(queue: "example-queue", exclusive: false);

//Queue'dan Mesaj Okuma
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue:"example-queue", autoAck:false, consumer);
consumer.Received += (sender , e) =>
{
    //Kuyruğa gelen mesajın işlendiği yer.
    //e.Body : Kuyruktaki mesajın verisini bütünsel olarak getiriir.
    //e.Body.Span veya e.Body.ToArray fonksiyonları kuyruktaki mesajın byte verisini getirecektir.
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};
Console.Read();