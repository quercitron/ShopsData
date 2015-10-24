using DataCollectorCore;
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
            var powerSupplyProductType = new ProductType { Name = ProductTypeName.PowerSupply };

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
            helper = (GeneralPowerSupplyProductRecordHelper)(new DnsSourceManager().GetProductRecordHelper(powerSupplyProductType));

            result = helper.ProcessName(new ProductRecord { Name = "Блок питания Chieftec A-135 Series 750W [APS-750CB]", Brand = "Chieftec" });
            Assert.That(result.Name, Is.EqualTo("Блок питания Chieftec APS-750CB"));

            result = helper.ProcessName(new ProductRecord { Name = "Блок питания FSP PNR 550W [ATX550-PNR]", Brand = "FSP" });
            Assert.That(result.Name, Is.EqualTo("Блок питания FSP 550PNR"));

            result = helper.ProcessName(new ProductRecord { Name = "Блок питания Corsair CX 500W [CP-9020047-EU]", Brand = "Corsair" });
            Assert.That(result.Name, Is.EqualTo("Блок питания Corsair CX 500"));

            result = helper.ProcessName(new ProductRecord { Name = "Блок питания Corsair VS 450W [CP-9020049-NA]", Brand = "Corsair" });
            Assert.That(result.Name, Is.EqualTo("Блок питания Corsair VS 450"));


            // key
            helper = (GeneralPowerSupplyProductRecordHelper)(new KeySourceManager().GetProductRecordHelper(powerSupplyProductType));

            result = helper.ProcessName(new ProductRecord { Name = "Блок питания БП Cooler Master V750 Semi-modular 750W ATX" });
            Assert.That(result.Name, Is.EqualTo("Блок питания Cooler Master V750"));

            result = helper.ProcessName(new ProductRecord { Name = "Блок питания БП FSP Group 500W ATX" });
            Assert.That(result.Name, Is.EqualTo("Блок питания FSP 500PNR"));

            result = helper.ProcessName(new ProductRecord { Name = "Блок питания FSP Group 300W SFX" });
            Assert.That(result.Name, Is.EqualTo("Блок питания FSP 300PNR SFX"));

            result = helper.ProcessName(new ProductRecord { Name = "Блок питания БП FSP Group Aurum Pro Gold 80 Plus 1200W ATX" });
            Assert.That(result.Name, Is.EqualTo("Блок питания FSP Aurum Pro Gold 80 Plus 1200PNR"));


            // ulmart
            helper = (GeneralPowerSupplyProductRecordHelper)(new UlmartSourceManager().GetProductRecordHelper(powerSupplyProductType));

            result = helper.ProcessName(new ProductRecord { Name = "блок питания ATX Corsair CX 500, CP-9020047-EU, 500W" });
            Assert.That(result.Name, Is.EqualTo("Блок питания Corsair CX 500"));

            result = helper.ProcessName(new ProductRecord { Name = "блок питания ATX Corsair AX860, CP-9020044-EU, 860W" });
            Assert.That(result.Name, Is.EqualTo("Блок питания Corsair AX860"));
        }

        [Test]
        public void ScrewdriverProductRecordHelperTest()
        {
            var screwdriverProductType = new ProductType { Name = ProductTypeName.Screwdriver };

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
            var motherboardProductType = new ProductType { Name = ProductTypeName.Motherboard };

            var helper = new GeneralMotherboardProductRecordHelper();

            var result = helper.ProcessName(new ProductRecord { Name = "серверная материнская плата ASUS Z9PE-D16" });
            Assert.That(result.Name, Is.EqualTo("Серверная материнская плата ASUS Z9PE-D16"));

            result = helper.ProcessName(new ProductRecord { Name = "MSI A68HM-E33" });
            Assert.That(result.Name, Is.EqualTo("Материнская плата MSI A68HM-E33"));

            // key
            helper = (GeneralMotherboardProductRecordHelper)(new KeySourceManager().GetProductRecordHelper(motherboardProductType));

            result = helper.ProcessName(new ProductRecord { Name = "Материнская плата MB Asus A58M-K" });
            Assert.That(result.Name, Is.EqualTo("Материнская плата Asus A58M-K"));

            result = helper.ProcessName(new ProductRecord { Name = "Материнская плата MB MSI A68HM-P33 V2" });
            Assert.That(result.Name, Is.EqualTo("Материнская плата MSI A68HM-P33 V2"));
        }

        [Test]
        public void MonitorProductRecordHelper()
        {
            var monitorProductType = new ProductType { Name = ProductTypeName.Monitor };

            var helper = new GeneralMonitorProductRecordHelper();

            var result = helper.ProcessName(new ProductRecord { Name = "Монитор ЖК PHILIPS 224E5QSB (00/01)" });
            Assert.That(result.Name, Is.EqualTo("Монитор PHILIPS 224E5QSB"));

            result = helper.ProcessName(new ProductRecord { Name = "21.5\" Монитор Philips 226V4LSB, 00(01) черный" });
            Assert.That(result.Name, Is.EqualTo("Монитор Philips 226V4LSB"));
            Assert.That(result.Class, Is.EqualTo("Black"));


            // key
            helper = (GeneralMonitorProductRecordHelper)(new KeySourceManager().GetProductRecordHelper(monitorProductType));

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

            result = helper.ProcessName(new ProductRecord { Name = "Монитор 23.5\" LG Flatron 24M37A-B Black" });
            Assert.That(result.Name, Is.EqualTo("Монитор LG 24M37A-B"));
            Assert.That(result.Class, Is.EqualTo("Black"));
        }

        [Test]
        public void SsdProductRecordHelper()
        {
            var ssdProductType = new ProductType { Name = ProductTypeName.SSD };

            // dns
            var helper = (GeneralSSDProductRecordHelper)(new DnsSourceManager().GetProductRecordHelper(ssdProductType));

            var result = helper.ProcessName(new ProductRecord
            {
                Name = "SSD накопитель A-Data XPG SX930 [ASX930SS3-120GM-C]",
                Description = "[2.5\", 120 Гб, SATA III, чтение - 560 Мбайт, с, запись - 460 Мбайт, с, JMicron JMF670H]"
            });
            Assert.That(result.Name, Is.EqualTo("SSD накопитель A-Data XPG SX930 120GB"));
            Assert.That(result.Code, Is.EqualTo("ASX930SS3-120GM-C"));

            // citilink
            helper = (GeneralSSDProductRecordHelper)(new CitilinkSourceManager().GetProductRecordHelper(ssdProductType));

            result = helper.ProcessName(new ProductRecord
            {
                Name = "Накопитель SSD INTEL 530 Series SSDSC2BW120A4K5 120Гб",
            });
            Assert.That(result.Name, Is.EqualTo("Накопитель SSD INTEL 530 120GB"));
            Assert.That(result.Code, Is.EqualTo("SSDSC2BW120A4K5"));

            result = helper.ProcessName(new ProductRecord
            {
                Name = "Накопитель SSD A-DATA Premier SP600 ASP600S3-64GM-C 64Гб",
            });
            Assert.That(result.Name, Is.EqualTo("Накопитель SSD A-DATA Premier SP600 64GB"));
            Assert.That(result.Code, Is.EqualTo("ASP600S3-64GM-C"));

            result = helper.ProcessName(new ProductRecord
            {
                Name = "Накопитель SSD INTEL 730 Series SSDSC2BP240G410 933255 240Гб",
            });
            Assert.That(result.Name, Is.EqualTo("Накопитель SSD INTEL 730 240GB"));
            Assert.That(result.Code, Is.EqualTo("SSDSC2BP240G410"));

            // ulmart
            helper = (GeneralSSDProductRecordHelper)(new UlmartSourceManager().GetProductRecordHelper(ssdProductType));

            result = helper.ProcessName(new ProductRecord
            {
                Name = "жесткий диск SSD 120ГБ, Intel 530, SSDSC2BW120A4K5",
            });
            Assert.That(result.Name, Is.EqualTo("жесткий диск SSD Intel 530 120GB"));
            Assert.That(result.Code, Is.EqualTo("SSDSC2BW120A4K5"));

            result = helper.ProcessName(new ProductRecord
            {
                Name = "жесткий диск SSD 120ГБ, SmartBuy Ignition 2, SB120GB-IGNT-25SAT3",
            });
            Assert.That(result.Name, Is.EqualTo("жесткий диск SSD SmartBuy Ignition 2 120GB"));
            Assert.That(result.Code, Is.EqualTo("SB120GB-IGNT-25SAT3"));

            // key
            helper = (GeneralSSDProductRecordHelper)(new KeySourceManager().GetProductRecordHelper(ssdProductType));

            result = helper.ProcessName(new ProductRecord
            {
                Name = "Накопитель SSD 2.5\" 512 Гб Corsair Force Series LX",
            });
            Assert.That(result.Name, Is.EqualTo("Накопитель SSD Corsair Force LX 512GB"));
            Assert.That(result.Code, Is.Null);

            result = helper.ProcessName(new ProductRecord
            {
                Name = "Накопитель SSD 2.5\" 240 Гб Intel 730 Series",
            });
            Assert.That(result.Name, Is.EqualTo("Накопитель SSD Intel 730 240GB"));
            Assert.That(result.Code, Is.Null);
        }

        [Test]
        public void CpuProductRecordHelper()
        {
            var cpuProductType = new ProductType { Name = ProductTypeName.CPU };

            // ulmart
            var helper = (GeneralCpuProductRecordHelper)(new UlmartSourceManager().GetProductRecordHelper(cpuProductType));

            var result = helper.ProcessName(new ProductRecord
            {
                Name = "процессор AMD A10-7850K Black Edition, AD785KXBJABOX, BOX",
            });
            Assert.That(result.Name, Is.EqualTo("Процессор AMD A10-7850K Black Edition BOX"));
            Assert.That(result.Code, Is.EqualTo("AD785KXBJABOX"));
        }
    }
}
