using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net;
using System.Collections.Specialized;
using System.Threading;
using Newtonsoft.Json;

namespace DummyVendingMachine
{
    public partial class DummyVendingMachine : Form
    {
        private const string TokenMachine1 = "00840076005b00070049003d00db00dc006f001a002e0081003d000f00f6001a";
        private const int IdMachine1 = 12;
        private const int IdRefresco = 2;
        private const int IdSlotRefresco = 2;
        private const int IdCerveza = 3;
        private const int IdSlotCerveza = 1;
        private const int IdGolosinas = 7;
        private const int IdSlotGolosinas = 4;
        private const int IdPipas = 6;
        private const int IdSlotPipas = 5;
        private const int IdCiruelas = 5;
        private const int IdSlotCiruelas = 6;
        private const int IdGofres = 8;
        private const int IdSlotGofres = 8;


        public DummyVendingMachine()
        {
            InitializeComponent();
            GetStartProductsPriceAsync();
            Control.CheckForIllegalCrossThreadCalls = false;
            ThreadStart checkServiceDelegate = new ThreadStart(getUpdateProductpriceAsync);
            Thread checkServiceThread = new Thread(checkServiceDelegate);
            checkServiceThread.Start();

        }

        /// <summary>
        /// Llamada httpPost cuando se realiza una venta en la máquina
        /// </summary>
        /// <param name="product"></param>
        /// <param name="idSlotProduct"></param>
        private void SetNewSell(int product, int idSlotProduct)
        {
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["idMachine"] = TokenMachine1;
                string address = "http://localhost:49906/api/Soldeds/PostNewSale/" + $"{IdMachine1}/{TokenMachine1}/{product}/{idSlotProduct}/{Count1Cent.Text}-{Count2Cent.Text}-{Count5Cent.Text}-{Count10Cent.Text}-{Count20Cent.Text}-{Count50Cent.Text}-{Count1Eur.Text}-{Count2Eur.Text}";
                client.UploadValues(address, values);
            }
        }


        /// <summary>
        /// Evento de venta de un Refresco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresco_Click(object sender, EventArgs e)
        {
            setRandomReturnedCoins();
            SetNewSell(IdRefresco, IdSlotRefresco);

        }

        /// <summary>
        /// Evento de venta de una cerveza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCerveza_Click(object sender, EventArgs e)
        {
            setRandomReturnedCoins();
            SetNewSell(IdCerveza, IdSlotCerveza);
        }
        
        /// <summary>
        /// Evento de venta de una golosina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGolosinas_Click(object sender, EventArgs e)
        {
            setRandomReturnedCoins();
            SetNewSell(IdGolosinas, IdSlotGolosinas);
        }

        /// <summary>
        /// Evento de venta de pipas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPipas_Click(object sender, EventArgs e)
        {
            setRandomReturnedCoins();
            SetNewSell(IdPipas, IdSlotPipas);
        }

        /// <summary>
        /// Evento de venta de una ciruela
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCiruela_Click(object sender, EventArgs e)
        {
            setRandomReturnedCoins();
            SetNewSell(IdCiruelas, IdSlotCiruelas);
        }

        /// <summary>
        /// Evento de venta de un gofre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Gofre_Click(object sender, EventArgs e)
        {
            setRandomReturnedCoins();
            SetNewSell(IdGofres, IdSlotGofres);
        }

        /// <summary>
        /// Genera Números aleatorios simulando la devolución del cambio al realizar una compra
        /// </summary>
        private void setRandomReturnedCoins()
        {
            Random rnd = new Random();

            Count2Eur.Text = rnd.Next(1, 8).ToString();
            Count1Eur.Text = rnd.Next(1, 9).ToString();
            Count50Cent.Text = rnd.Next(1, 10).ToString();
            Count20Cent.Text = rnd.Next(1, 10).ToString();
            Count10Cent.Text = rnd.Next(1, 10).ToString();
            Count5Cent.Text = rnd.Next(1, 10).ToString();
            Count2Cent.Text = rnd.Next(1, 10).ToString();
            Count1Cent.Text = rnd.Next(1, 10).ToString();
        }


        /// <summary>
        /// Obtiene el precio inicial al conectar la máquina
        /// </summary>
        /// <returns></returns>
        private void GetStartProductsPriceAsync()
        {
            getProductpriceAsync(IdSlotRefresco, IdSlotRefresco, RefrescoPrice);
            getProductpriceAsync(IdSlotGolosinas, IdGolosinas, GolosinasPrice);
            getProductpriceAsync(IdSlotGofres, IdGofres, GofrePrice);
            getProductpriceAsync(IdSlotCerveza, IdCerveza, CervezaPrice);
            getProductpriceAsync(IdSlotPipas, IdPipas, PipasPrice);
            getProductpriceAsync(IdSlotCiruelas, IdCiruelas, CiruelaPrice);

        }

        /// <summary>
        /// Obtiene el precio de un producto
        /// </summary>
        private static readonly HttpClient client = new HttpClient();
        private async void getProductpriceAsync(int idSlot, int idProduct, TextBox textBox)
        {
            string conexion = "http://localhost:49906/api/Slots/GetSlots" + $"/{IdMachine1}/{TokenMachine1}/{idSlot}/{idProduct}";
            textBox.Text = await client.GetStringAsync(conexion);
        }

        /// <summary>
        /// Obtiene la actualización del precio de un producto
        /// </summary>
        private void getUpdateProductpriceAsync()
        {
            while (true)
            {
                //cada 8 horas
                Thread.Sleep(28800000);
                getUpdateProductpriceAsync(IdMachine1, TokenMachine1);
            }
        }

        /// <summary>
        /// Obtiene el precio de un producto
        /// </summary>
        private async void getUpdateProductpriceAsync(int IdMachine1, string TokenMachine1)
        {
            try
            {
                string conexion = "http://localhost:49906/api/Slots/GetUpdateSlots" + $"/{IdMachine1}/{TokenMachine1}";

                var updatePriceJson = await client.GetStringAsync(conexion);

                List<elementsToUpdate> itemJsonUpdate = JsonConvert.DeserializeObject<List<elementsToUpdate>>(updatePriceJson);
                UpdatePrice(itemJsonUpdate);
            }
            catch (Exception e) { }

        }

        /// <summary>
        /// Actualiza los precios de las máquinas cuando se detecta un cambio
        /// </summary>
        /// <param name="Slot"></param>
        private void UpdatePrice(List<elementsToUpdate> Slot)
        {
           foreach (var idSlotUpdate in Slot)
            {
                switch (idSlotUpdate.IdSlot)
                {
                    case IdRefresco:
                        RefrescoPrice.Text = idSlotUpdate.NewPrice.ToString();
                        continue;
                    case IdCerveza:
                        CervezaPrice.Text = idSlotUpdate.NewPrice.ToString();
                        continue;
                    case IdCiruelas:
                        CiruelaPrice.Text = idSlotUpdate.NewPrice.ToString();
                        continue;
                    case IdGofres:
                        GofrePrice.Text = idSlotUpdate.NewPrice.ToString();
                        continue;
                    case IdGolosinas:
                        GolosinasPrice.Text = idSlotUpdate.NewPrice.ToString();
                        continue;
                    case IdPipas:
                        PipasPrice.Text = idSlotUpdate.NewPrice.ToString();
                        continue;
                }
            }
        }

        /// <summary>
        /// Clase interna que representa un Slot que se va a actualizar el precio
        /// </summary>
        internal class elementsToUpdate
        {
            public int IdMachine { get; set; }
            public int IdProduct { get; set; }
            public int IdSlot { get; set; }
            public decimal NewPrice { get; set; }
        }
    }
}