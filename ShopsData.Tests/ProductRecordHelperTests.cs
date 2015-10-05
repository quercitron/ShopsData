using DataCollectorCore.DataObjects;

using DataCollectorFramework;

using NUnit.Framework;

namespace ShopsData.Tests
{
    [TestFixture]
    public class ProductRecordHelperTests
    {
        [Test]
        public void PowerSupplyProductRecordHelperTest()
        {
            var helper = new GeneralPowerSupplyProductRecordHelper();

            var result = helper.ProcessName(new ProductRecord { Name = "Блок питания HIPRO HPE400W" });
            Assert.That(result.Name, Is.EqualTo("Блок питания HIPRO HPE400"));

            result = helper.ProcessName(new ProductRecord { Name = "Блок питания HIPRO HPA-500W" });
            Assert.That(result.Name, Is.EqualTo("Блок питания HIPRO HPA-500"));

            result = helper.ProcessName(new ProductRecord { Name = "блок питания ATX 550W Corsair RM550" });
            Assert.That(result.Name, Is.EqualTo("Блок питания Corsair RM550"));

            result = helper.ProcessName(new ProductRecord { Name = "блок питания ATX FSP ATX-300PNR 300W" });
            Assert.That(result.Name, Is.EqualTo("Блок питания FSP 300PNR"));

            result = helper.ProcessName(new ProductRecord { Name = "блок питания ATX 450W Storm 45SHB" });
            Assert.That(result.Name, Is.EqualTo("Блок питания Storm 45SHB 450"));

            result = helper.ProcessName(new ProductRecord { Name = "БП Thermaltake ToughPower Grand 1200W ATX" });
            Assert.That(result.Name, Is.EqualTo("Блок питания Thermaltake ToughPower Grand 1200"));

            result = helper.ProcessName(new ProductRecord { Name = "Блок питания БП Thermaltake Russian Gold Baikal 1500W ATX" });
            Assert.That(result.Name, Is.EqualTo("Блок питания Thermaltake Russian Gold Baikal 1500"));

            // dns
            helper = new DnsPowerSupplyHelper();

            result = helper.ProcessName(new ProductRecord { Name = "Блок питания Chieftec A-135 Series 750W [APS-750CB]", Brand = "Chieftec" });
            Assert.That(result.Name, Is.EqualTo("Блок питания Chieftec APS-750CB"));

            result = helper.ProcessName(new ProductRecord { Name = "Блок питания FSP PNR 550W [ATX550-PNR]", Brand = "FSP" });
            Assert.That(result.Name, Is.EqualTo("Блок питания FSP 550PNR"));
        }

        [Test]
        public void ScrewdriverProductRecordHelperTest()
        {
            var helper = new GeneralScrewdriverProductRecordHelper();

            var result = helper.ProcessName(new ProductRecord { Name = "аккумуляторный шуруповерт Bosch PSR 14,4 Li (0.603.954.322)" });
            Assert.That(result.Name, Is.EqualTo("Шуруповерт Bosch PSR 14,4 Li"));

            result = helper.ProcessName(new ProductRecord { Name = "аккумуляторный гайковерт DWT ABW-18 SLi BMC" });
            Assert.That(result.Name, Is.EqualTo("Гайковерт DWT ABW-18 SLi BMC"));

            result = helper.ProcessName(new ProductRecord { Name = "Ударная дрель-шуруповерт MAKITA HP330DX100" });
            Assert.That(result.Name, Is.EqualTo("Шуруповерт ударный MAKITA HP330DX100"));

            result = helper.ProcessName(new ProductRecord { Name = "Дрель-шуруповерт ИНТЕРСКОЛ ДА-10/12М2" });
            Assert.That(result.Name, Is.EqualTo("Шуруповерт INTERSKOL ДА-10/12М2"));

            result = helper.ProcessName(new ProductRecord { Name = "Дрель-шуруповерт BOSCH PSR 10.8 LI-2 (без аккумулятора и з/у)" });
            Assert.That(result.Name, Is.EqualTo("Шуруповерт BOSCH PSR 10.8 LI-2 (without battery and charger)"));
        }

        [Test]
        public void MotherboardProductRecordHelper()
        {
            var helper = new GeneralMotherboardProductRecordHelper();

            var result = helper.ProcessName(new ProductRecord { Name = "серверная материнская плата ASUS Z9PE-D16" });
            Assert.That(result.Name, Is.EqualTo("Серверная материнская плата ASUS Z9PE-D16"));

            result = helper.ProcessName(new ProductRecord { Name = "MSI A68HM-E33" });
            Assert.That(result.Name, Is.EqualTo("Материнская плата MSI A68HM-E33"));

            // key
            helper = new KeyMotherboardHelper();

            result = helper.ProcessName(new ProductRecord { Name = "Материнская плата MB Asus A58M-K" });
            Assert.That(result.Name, Is.EqualTo("Материнская плата Asus A58M-K"));
        }

        [Test]
        public void MonitorProductRecordHelper()
        {
            var helper = new GeneralMonitorProductRecordHelper();

            var result = helper.ProcessName(new ProductRecord { Name = "Монитор ЖК PHILIPS 224E5QSB (00/01)" });
            Assert.That(result.Name, Is.EqualTo("Монитор PHILIPS 224E5QSB"));

            result = helper.ProcessName(new ProductRecord { Name = "21.5\" Монитор Philips 226V4LSB, 00(01) черный" });
            Assert.That(result.Name, Is.EqualTo("Монитор Philips 226V4LSB"));
            Assert.That(result.Class, Is.EqualTo("Black"));


            // key
            helper = new KeyMonitorHelper();

            result = helper.ProcessName(new ProductRecord { Name = "Монитор Acer G246HYLbmjj Black" });
            Assert.That(result.Name, Is.EqualTo("Монитор Acer G246HYLbmjj"));
            Assert.That(result.Class, Is.EqualTo("Black"));

            result = helper.ProcessName(new ProductRecord { Name = "Монитор LG 34UM95-P Black&Silver" });
            Assert.That(result.Name, Is.EqualTo("Монитор LG 34UM95-P"));
            Assert.That(result.Class, Is.EqualTo("Silver Black"));

            result = helper.ProcessName(new ProductRecord { Name = "Монитор LG 22MP56HQ-T Black/Silver" });
            Assert.That(result.Name, Is.EqualTo("Монитор LG 22MP56HQ-T"));
            Assert.That(result.Class, Is.EqualTo("Silver Black"));

            result = helper.ProcessName(new ProductRecord { Name = "Монитор Philips 223V5QHSB6/ Black Hairline" });
            Assert.That(result.Name, Is.EqualTo("Монитор Philips 223V5QHSB6"));
            Assert.That(result.Class, Is.EqualTo("Black Hairline"));
        }
    }
}
