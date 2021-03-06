﻿// Accord Unit Tests
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © César Souza, 2009-2015
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

namespace Accord.Tests.Statistics
{
    using Accord.Statistics.Analysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Accord.Statistics.Kernels;
    using Accord.Math;
    using System.Diagnostics;
    using Accord.IO;
    using System.Data;

    [TestClass()]
    public class KernelPrincipalComponentAnalysisTest
    {

        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }



        // Lindsay's tutorial data
        private static double[,] data =
        {
            { 2.5,  2.4 },
            { 0.5,  0.7 },
            { 2.2,  2.9 },
            { 1.9,  2.2 },
            { 3.1,  3.0 },
            { 2.3,  2.7 },
            { 2.0,  1.6 },
            { 1.0,  1.1 },
            { 1.5,  1.6 },
            { 1.1,  0.9 }
        };


        [TestMethod()]
        public void TransformTest()
        {
            // Using a linear kernel should be equivalent to standard PCA
            IKernel kernel = new Linear();

            // Create analysis
            var target = new KernelPrincipalComponentAnalysis(data, kernel, AnalysisMethod.Center);

            // Compute
            target.Compute();

            double[,] actual = target.Transform(data, 2);

            // first inversed.. ?
            double[,] expected = new double[,]
            {
                { -0.827970186,  0.175115307 },
                {  1.77758033,  -0.142857227 },
                { -0.992197494, -0.384374989 },
                { -0.274210416, -0.130417207 },
                { -1.67580142,   0.209498461 },
                { -0.912949103, -0.175282444 },
                {  0.099109437,  0.349824698 },
                {  1.14457216,  -0.046417258 },
                {  0.438046137, -0.017764629 },
                {  1.22382056,   0.162675287 },
            };

            // Verify both are equal with 0.001 tolerance value
            Assert.IsTrue(Matrix.IsEqual(actual, expected, 0.0001));

            // Assert the result equals the transformation of the input
            double[,] result = target.Result;
            double[,] projection = target.Transform(data);
            Assert.IsTrue(Matrix.IsEqual(result, projection, 0.000001));
        }

        [TestMethod()]
        public void TransformTest2()
        {
            // Using a linear kernel should be equivalent to standard PCA
            IKernel kernel = new Linear();

            // Create analysis
            KernelPrincipalComponentAnalysis target =
                new KernelPrincipalComponentAnalysis(data, kernel, AnalysisMethod.Center);

            // Set the minimum variance threshold to 0.001
            target.Threshold = 0.001;

            // Compute
            target.Compute();

            var r = target.Result;

            double[,] actual = target.Transform(data, 1);

            // first inversed.. ?
            double[,] expected = new double[,]
            {
                { -0.827970186 },
                {  1.77758033  },
                { -0.992197494 },
                { -0.274210416 },
                { -1.67580142  },
                { -0.912949103 },
                {  0.099109437 },
                {  1.14457216  },
                {  0.438046137 },
                {  1.22382056  },
            };

            // Verify both are equal with 0.001 tolerance value
            Assert.IsTrue(Matrix.IsEqual(actual, expected, 0.0001));

            // Assert the result equals the transformation of the input
            double[,] result = target.Result;
            double[,] projection = target.Transform(data);
            Assert.IsTrue(Matrix.IsEqual(result, projection, 0.000001));
        }

        [TestMethod()]
        public void TransformTest3()
        {
            // Using a linear kernel should be equivalent to standard PCA
            IKernel kernel = new Linear();

            // Create analysis
            KernelPrincipalComponentAnalysis target = new KernelPrincipalComponentAnalysis(data, kernel);

            // Compute
            target.Compute();

            bool thrown = false;
            try
            {
                double[,] actual = target.Transform(data, 11);
            }
            catch
            {
                thrown = true;
            }

            Assert.IsTrue(thrown);
        }

        [TestMethod()]
        public void TransformTest4()
        {
            // Tested against R's kernlab

            // test <-   c(16, 117, 94, 132, 13, 73, 68, 129, 91, 50, 56, 12, 145, 105, 35, 53, 38, 51, 85, 116)
            int[] test = { 15, 116, 93, 131, 12, 72, 67, 128, 90, 49, 55, 11, 144, 104, 34, 52, 37, 50, 84, 115 };

            // data(iris)
            double[,] iris =
            {
                #region Fisher's iris dataset
                { 5.1, 3.5,  1.4, 0.2, 0 },
                { 4.9, 3.0,  1.4, 0.2, 0 },
                { 4.7, 3.2,  1.3, 0.2, 0 },
                { 4.6, 3.1,  1.5, 0.2, 0 },
                { 5.0, 3.6,  1.4, 0.2, 0 },
                { 5.4, 3.9,  1.7, 0.4, 0 },
                { 4.6, 3.4,  1.4, 0.3, 0 },
                { 5.0, 3.4,  1.5, 0.2, 0 },
                { 4.4, 2.9,  1.4, 0.2, 0 },
                { 4.9, 3.1,  1.5, 0.1, 0 },
                { 5.4, 3.7,  1.5, 0.2, 0 },
                { 4.8, 3.4,  1.6, 0.2, 0 },
                { 4.8, 3.0,  1.4, 0.1, 0 },
                { 4.3, 3.0,  1.1, 0.1, 0 },
                { 5.8, 4.0,  1.2, 0.2, 0 },
                { 5.7, 4.4,  1.5, 0.4, 0 },
                { 5.4, 3.9,  1.3, 0.4, 0 },
                { 5.1, 3.5,  1.4, 0.3, 0 },
                { 5.7, 3.8,  1.7, 0.3, 0 },
                { 5.1, 3.8,  1.5, 0.3, 0 },
                { 5.4, 3.4,  1.7, 0.2, 0 },
                { 5.1, 3.7,  1.5, 0.4, 0 },
                { 4.6, 3.6,  1.0, 0.2, 0 },
                { 5.1, 3.3,  1.7, 0.5, 0 },
                { 4.8, 3.4,  1.9, 0.2, 0 },
                { 5.0, 3.0,  1.6, 0.2, 0 },
                { 5.0, 3.4,  1.6, 0.4, 0 },
                { 5.2, 3.5,  1.5, 0.2, 0 },
                { 5.2, 3.4,  1.4, 0.2, 0 },
                { 4.7, 3.2,  1.6, 0.2, 0 },
                { 4.8, 3.1,  1.6, 0.2, 0 },
                { 5.4, 3.4,  1.5, 0.4, 0 },
                { 5.2, 4.1,  1.5, 0.1, 0 },
                { 5.5, 4.2,  1.4, 0.2, 0 },
                { 4.9, 3.1,  1.5, 0.2, 0 },
                { 5.0, 3.2,  1.2, 0.2, 0 },
                { 5.5, 3.5,  1.3, 0.2, 0 },
                { 4.9, 3.6,  1.4, 0.1, 0 },
                { 4.4, 3.0,  1.3, 0.2, 0 },
                { 5.1, 3.4,  1.5, 0.2, 0 },
                { 5.0, 3.5,  1.3, 0.3, 0 },
                { 4.5, 2.3,  1.3, 0.3, 0 },
                { 4.4, 3.2,  1.3, 0.2, 0 },
                { 5.0, 3.5,  1.6, 0.6, 0 },
                { 5.1, 3.8,  1.9, 0.4, 0 },
                { 4.8, 3.0,  1.4, 0.3, 0 },
                { 5.1, 3.8,  1.6, 0.2, 0 },
                { 4.6, 3.2,  1.4, 0.2, 0 },
                { 5.3, 3.7,  1.5, 0.2, 0 },
                { 5.0, 3.3,  1.4, 0.2, 0 },
                { 7.0, 3.2,  4.7, 1.4, 1 },
                { 6.4, 3.2,  4.5, 1.5, 1 },
                { 6.9, 3.1,  4.9, 1.5, 1 },
                { 5.5, 2.3,  4.0, 1.3, 1 },
                { 6.5, 2.8,  4.6, 1.5, 1 },
                { 5.7, 2.8,  4.5, 1.3, 1 },
                { 6.3, 3.3,  4.7, 1.6, 1 },
                { 4.9, 2.4,  3.3, 1.0, 1 },
                { 6.6, 2.9,  4.6, 1.3, 1 },
                { 5.2, 2.7,  3.9, 1.4, 1 },
                { 5.0, 2.0,  3.5, 1.0, 1 },
                { 5.9, 3.0,  4.2, 1.5, 1 },
                { 6.0, 2.2,  4.0, 1.0, 1 },
                { 6.1, 2.9,  4.7, 1.4, 1 },
                { 5.6, 2.9,  3.6, 1.3, 1 },
                { 6.7, 3.1,  4.4, 1.4, 1 },
                { 5.6, 3.0,  4.5, 1.5, 1 },
                { 5.8, 2.7,  4.1, 1.0, 1 },
                { 6.2, 2.2,  4.5, 1.5, 1 },
                { 5.6, 2.5,  3.9, 1.1, 1 },
                { 5.9, 3.2,  4.8, 1.8, 1 },
                { 6.1, 2.8,  4.0, 1.3, 1 },
                { 6.3, 2.5,  4.9, 1.5, 1 },
                { 6.1, 2.8,  4.7, 1.2, 1 },
                { 6.4, 2.9,  4.3, 1.3, 1 },
                { 6.6, 3.0,  4.4, 1.4, 1 },
                { 6.8, 2.8,  4.8, 1.4, 1 },
                { 6.7, 3.0,  5.0, 1.7, 1 },
                { 6.0, 2.9,  4.5, 1.5, 1 },
                { 5.7, 2.6,  3.5, 1.0, 1 },
                { 5.5, 2.4,  3.8, 1.1, 1 },
                { 5.5, 2.4,  3.7, 1.0, 1 },
                { 5.8, 2.7,  3.9, 1.2, 1 },
                { 6.0, 2.7,  5.1, 1.6, 1 },
                { 5.4, 3.0,  4.5, 1.5, 1 },
                { 6.0, 3.4,  4.5, 1.6, 1 },
                { 6.7, 3.1,  4.7, 1.5, 1 },
                { 6.3, 2.3,  4.4, 1.3, 1 },
                { 5.6, 3.0,  4.1, 1.3, 1 },
                { 5.5, 2.5,  4.0, 1.3, 1 },
                { 5.5, 2.6,  4.4, 1.2, 1 },
                { 6.1, 3.0,  4.6, 1.4, 1 },
                { 5.8, 2.6,  4.0, 1.2, 1 },
                { 5.0, 2.3,  3.3, 1.0, 1 },
                { 5.6, 2.7,  4.2, 1.3, 1 },
                { 5.7, 3.0,  4.2, 1.2, 1 },
                { 5.7, 2.9,  4.2, 1.3, 1 },
                { 6.2, 2.9,  4.3, 1.3, 1 },
                { 5.1, 2.5,  3.0, 1.1, 1 },
                { 5.7, 2.8,  4.1, 1.3, 1 },
                { 6.3, 3.3,  6.0, 2.5, 2 },
                { 5.8, 2.7,  5.1, 1.9, 2 },
                { 7.1, 3.0,  5.9, 2.1, 2 },
                { 6.3, 2.9,  5.6, 1.8, 2 },
                { 6.5, 3.0,  5.8, 2.2, 2 },
                { 7.6, 3.0,  6.6, 2.1, 2 },
                { 4.9, 2.5,  4.5, 1.7, 2 },
                { 7.3, 2.9,  6.3, 1.8, 2 },
                { 6.7, 2.5,  5.8, 1.8, 2 },
                { 7.2, 3.6,  6.1, 2.5, 2 },
                { 6.5, 3.2,  5.1, 2.0, 2 },
                { 6.4, 2.7,  5.3, 1.9, 2 },
                { 6.8, 3.0,  5.5, 2.1, 2 },
                { 5.7, 2.5,  5.0, 2.0, 2 },
                { 5.8, 2.8,  5.1, 2.4, 2 },
                { 6.4, 3.2,  5.3, 2.3, 2 },
                { 6.5, 3.0,  5.5, 1.8, 2 },
                { 7.7, 3.8,  6.7, 2.2, 2 },
                { 7.7, 2.6,  6.9, 2.3, 2 },
                { 6.0, 2.2,  5.0, 1.5, 2 },
                { 6.9, 3.2,  5.7, 2.3, 2 },
                { 5.6, 2.8,  4.9, 2.0, 2 },
                { 7.7, 2.8,  6.7, 2.0, 2 },
                { 6.3, 2.7,  4.9, 1.8, 2 },
                { 6.7, 3.3,  5.7, 2.1, 2 },
                { 7.2, 3.2,  6.0, 1.8, 2 },
                { 6.2, 2.8,  4.8, 1.8, 2 },
                { 6.1, 3.0,  4.9, 1.8, 2 },
                { 6.4, 2.8,  5.6, 2.1, 2 },
                { 7.2, 3.0,  5.8, 1.6, 2 },
                { 7.4, 2.8,  6.1, 1.9, 2 },
                { 7.9, 3.8,  6.4, 2.0, 2 },
                { 6.4, 2.8,  5.6, 2.2, 2 },
                { 6.3, 2.8,  5.1, 1.5, 2 },
                { 6.1, 2.6,  5.6, 1.4, 2 },
                { 7.7, 3.0,  6.1, 2.3, 2 },
                { 6.3, 3.4,  5.6, 2.4, 2 },
                { 6.4, 3.1,  5.5, 1.8, 2 },
                { 6.0, 3.0,  4.8, 1.8, 2 },
                { 6.9, 3.1,  5.4, 2.1, 2 },
                { 6.7, 3.1,  5.6, 2.4, 2 },
                { 6.9, 3.1,  5.1, 2.3, 2 },
                { 5.8, 2.7,  5.1, 1.9, 2 },
                { 6.8, 3.2,  5.9, 2.3, 2 },
                { 6.7, 3.3,  5.7, 2.5, 2 },
                { 6.7, 3.0,  5.2, 2.3, 2 },
                { 6.3, 2.5,  5.0, 1.9, 2 },
                { 6.5, 3.0,  5.2, 2.0, 2 },
                { 6.2, 3.4,  5.4, 2.3, 2 },
                { 5.9, 3.0,  5.1, 1.8, 2 },
                #endregion
            };


            // kpc <- kpca(~.,data=iris[-test,-5],kernel="rbfdot",kpar=list(sigma=0.2),features=2)
            var data = iris.Remove(test, new[] { 4 });
            var kernel = new Gaussian() { Gamma = 1 };
            var kpc = new KernelPrincipalComponentAnalysis(data, kernel);

            kpc.Compute(2);

            var rotated = kpc.Result;
            var pcv = kpc.ComponentMatrix;
            var eig = kpc.Eigenvalues;

            double[] expected_eig = { 28.542404060412132, 15.235596653653861 };
            double[,] expected_pcv = 
            {
                #region expected PCV without R's / m normalization
                { -0.0266876243479222, -0.00236424647855596 },
                { -0.0230827502249994, -0.00182207284533632 },
                { -0.0235846044938792, -0.00184417084258023 },
                { -0.0219741114149703, -0.00162806197434679 },
                { -0.0262369254935451, -0.00228351232176506 },
                { -0.0194129527129315, -0.00128157547584046 },
                { -0.0233710173690426, -0.00183018780267092 },
                { -0.0270621345426091, -0.00244551460941156 },
                { -0.01660360115437, -0.000759995006404066 },
                { -0.0241543595644871, -0.00200028851593623 },
                { -0.0229396684027426, -0.00178791975184668 },
                { -0.0141003945759371, -0.000349601250510858 },
                { -0.0122801944616023, -0.00011745003303124 },
                { -0.0195599909514198, -0.00123924882430174 },
                { -0.0267285199984888, -0.00237835151986576 },
                { -0.0164051441608544, -0.000826433392421186 },
                { -0.0241385103747907, -0.00196921837902471 },
                { -0.0228276819006861, -0.00190280719845395 },
                { -0.0250090125071634, -0.00212322482498387 },
                { -0.018268574949505, -0.00103729989327242 },
                { -0.0239555047501124, -0.00216590896337712 },
                { -0.0218837974825259, -0.00180921340210779 },
                { -0.0228699114274226, -0.00189079843025579 },
                { -0.0262414571955617, -0.00238666692022459 },
                { -0.02628286882499, -0.00232756740052467 },
                { -0.0261369413490628, -0.00229661111040973 },
                { -0.0237893959503383, -0.00195315162891338 },
                { -0.0237354902927562, -0.00197089686864334 },
                { -0.0234996712936547, -0.0019545398678434 },
                { -0.0179796342021205, -0.00100004281923827 },
                { -0.0143171193045046, -0.000421228427584423 },
                { -0.0241702773143143, -0.00196078350204665 },
                { -0.0215781675204649, -0.00158425565875557 },
                { -0.0174405137049866, -0.000872162100068597 },
                { -0.0268982927662575, -0.00242925852652081 },
                { -0.0261930727206913, -0.00227548913089953 },
                { -0.00807266421494459, 0.000505384268295841 },
                { -0.0189832707329651, -0.00111618515851182 },
                { -0.023789443724428, -0.00203239968623136 },
                { -0.0201457716377357, -0.00150731437393246 },
                { -0.0226387870826046, -0.00174799717649726 },
                { -0.0237772220904885, -0.00192536309172948 },
                { -0.0227864577886965, -0.00172757669197999 },
                { -0.0241368046325238, -0.00197147776349598 },
                { 0.0162307596401467, -0.00932217153629181 },
                { 0.00924104683890504, -0.0371256298132236 },
                { 0.0172460604733757, -0.00601678602419225 },
                { 0.0164784470762724, -0.00012129053123478 },
                { 0.00225808467595593, -0.0155701555363185 },
                { 0.0152659569368524, -0.00695503994803249 },
                { 0.00795619200816849, -0.034188555904496 },
                { 0.00255986394744671, -0.0156335839305463 },
                { 0.0157735235376026, -0.0339711483141172 },
                { 0.00860192955815661, -0.0310332456913026 },
                { 0.0188286198627367, -0.0143146603067418 },
                { 0.0081385823965042, -0.0358483794263587 },
                { 0.0131727085950618, -0.00748671161967017 },
                { 0.0150373592446138, -0.0269773780381651 },
                { 0.0126779242124717, -0.0162727482334416 },
                { 0.00983265072294127, -0.0416039968698012 },
                { 0.0162669562079483, -0.000151449934923387 },
                { 0.0137854766363786, -0.0375070307423622 },
                { 0.0170058660389757, -0.0184237621007135 },
                { 0.0154946725649067, -0.0227889410670457 },
                { 0.014708096275464, -0.011169199019916 },
                { 0.0135541309647514, 0.00627293040317239 },
                { 0.0153982178833786, 0.0228745884070871 },
                { 0.0186116855914761, -0.0238281923214434 },
                { 0.00661605660296714, -0.0332168101819555 },
                { 0.00812230548276198, -0.0380947707628449 },
                { 0.00704157480127114, -0.0353293378234606 },
                { 0.0118813500247593, -0.0433955442329169 },
                { 0.0168403649935284, 0.00717417511929008 },
                { 0.0144885311444922, -0.0128879186387195 },
                { 0.0148385454088314, 0.00481616750741218 },
                { 0.0127847825706042, -0.0211295878510692 },
                { 0.0126141523297424, -0.0394948238730571 },
                { 0.0105804278587419, -0.0411832808826231 },
                { 0.0185081272399827, -0.0181339486962481 },
                { 0.0124993892884636, -0.0434407731971394 },
                { 0.0135227934497893, -0.0415894662412569 },
                { 0.0136028421755366, -0.0388446289823116 },
                { 0.0144604273990706, -0.0404041262573942 },
                { 0.0165646866155949, -0.0294021220435322 },
                { 0.00146858312783178, -0.0134333124454357 },
                { 0.0137785343752508, -0.0429733697468562 },
                { 0.00510997410924024, 0.0292833047881736 },
                { 0.014720812085274, 0.00944264118212137 },
                { 0.00598583015620509, 0.038742545754176 },
                { 0.0125544895333347, 0.0349170237345097 },
                { 0.00140911493699792, 0.0164126558963803 },
                { 0.00546764790022381, -0.0140904440836446 },
                { 0.0029496609416271, 0.0249945804373137 },
                { 0.00769932014045035, 0.0313261912102264 },
                { 0.00266139821119332, 0.0231665860038695 },
                { 0.0147368620502789, 0.0315131740192214 },
                { 0.0149582869669828, 0.0314622232024109 },
                { 0.0106818628524054, 0.0443273862959601 },
                { 0.0123400540017047, 0.00422397833506881 },
                { 0.0101542521522688, 0.0157643916651046 },
                { 0.000660568495239385, 0.0087957765410289 },
                { 0.000634971613911479, 0.00896839373841372 },
                { 0.0119909422310846, -0.0019235494568038 },
                { 0.00742254354227651, 0.0421145349479265 },
                { 0.0130658707704511, 0.000658712215109605 },
                { 0.00103199141821948, 0.0130131637684367 },
                { 0.0180388007633923, 0.0112135357385706 },
                { 0.00879897568153878, 0.0428371609763469 },
                { 0.00466754803065601, 0.0321456973019424 },
                { 0.0188135431637204, 0.00458127473828957 },
                { 0.0184728744733845, 0.00843677964296344 },
                { 0.0055676853191067, 0.0305087649038716 },
                { 0.0033635667326866, 0.026834775073324 },
                { 0.0108405706484462, 0.0394739066547236 },
                { 0.0172770225385115, 0.0124967454210229 },
                { 0.0100507351970463, 0.0166565450918105 },
                { 0.00209404741665691, 0.0205532162586405 },
                { 0.0078782378323636, 0.0341148825697675 },
                { 0.0132731813046484, 0.0368540207320806 },
                { 0.0182550250587539, 0.000797957664175355 },
                { 0.0102561686092287, 0.0420705939254378 },
                { 0.00857331992305152, 0.0423810139397453 },
                { 0.00964648674506066, 0.0337591223497657 },
                { 0.014720812085274, 0.00944264118212137 },
                { 0.00659947194015947, 0.0404655648392282 },
                { 0.011337029514041, 0.0378339231578959 },
                { 0.0154602034267052, 0.0153085911335171 },
                { 0.0152371977428677, 0.0355309408870963 },
                { 0.0096520854263212, 0.0316677099444034 },
                { 0.016280981143395, 0.011860068380509 } 
#endregion
            };

            Assert.IsTrue(Matrix.IsEqual(expected_eig, eig, 1e-10));
            Assert.IsTrue(Matrix.IsEqual(expected_pcv, pcv, 1e-10));

            double[,] irisSubset =
            {
                #region Iris subset for testing
                { 5.7,  4.4,  1.5,  0.4 },
                { 6.5,  3.0,  5.5,  1.8 },
                { 5.0,  2.3,  3.3,  1.0 },
                { 7.9,  3.8,  6.4,  2.0 },
                { 4.8,  3.0,  1.4,  0.1 },
                { 6.3,  2.5,  4.9,  1.5 },
                { 5.8,  2.7,  4.1,  1.0 },
                { 6.4,  2.8,  5.6,  2.1 },
                { 5.5,  2.6,  4.4,  1.2 },
                { 5.0,  3.3,  1.4,  0.2 },
                { 5.7,  2.8,  4.5,  1.3 },
                { 4.8,  3.4,  1.6,  0.2 },
                { 6.7,  3.3,  5.7,  2.5 },
                { 6.5,  3.0,  5.8,  2.2 },
                { 4.9,  3.1,  1.5,  0.2 },
                { 6.9,  3.1,  4.9,  1.5 },
                { 4.9,  3.6,  1.4,  0.1 },
                { 7.0,  3.2,  4.7,  1.4 },
                { 5.4,  3.0,  4.5,  1.5 },
                { 6.4,  3.2,  5.3,  2.3 },
                #endregion
            };

            var testing = iris.Submatrix(test, new[] { 0, 1, 2, 3 });

            Assert.IsTrue(Matrix.IsEqual(irisSubset, testing));


            double[,] proj = kpc.Transform(testing);

            double[,] expectedProjection =
            {
                #region expected projection without R's /m normalization
                { -0.262045246354652, 0.00522592803297944 },
                { 0.379100878015288, 0.588655883281293 },
                { 0.071459976634984, -0.256289120689712 },
                { 0.0208479105625222, 0.139976227370984 },
                { -0.634360697174036, -0.0253624940325211 },
                { 0.467841054198416, 0.0142078237631541 },
                { 0.3464723948387, -0.62695357333265 },
                { 0.327328144102457, 0.603762286061834 },
                { 0.351964514241981, -0.539035845068089 },
                { -0.759054821003877, -0.035920361137046 },
                { 0.449638018323254, -0.492049890038061 },
                { -0.732335083049923, -0.0341252836840602 },
                { 0.192183096200302, 0.580343336854431 },
                { 0.256170478557119, 0.639157216957949 },
                { -0.703212303846621, -0.0317868463626801 },
                { 0.3515430820112, 0.224868844202495 },
                { -0.722813976459246, -0.0325608519534802 },
                { 0.286990265042346, 0.102161459040097 },
                { 0.354904620698745, -0.390810675482863 },
                { 0.332125880099634, 0.566660263312128 } 
                #endregion
            };

            Assert.IsTrue(expectedProjection.IsEqual(proj, 1e-6));

        }

        [TestMethod()]
        public void TransformTest5()
        {
            int element = 10;
            int dimension = 20;

            double[,] data = new double[element, dimension];

            int x = 0;

            for (int i = 0; i < element; i++)
            {
                for (int j = 0; j < dimension; j++)
                    data[i, j] = x;

                x += 10;
            }

            IKernel kernel = new Gaussian(10.0);
            var kpca = new KernelPrincipalComponentAnalysis(data, kernel, AnalysisMethod.Center);

            kpca.Compute();

            double[,] result = kpca.Transform(data, 2);

            double[,] expected = new double[,] 
            {
                { -0.23053882357602, -0.284413654763538 },
                { -0.387883199575312, -0.331485820285834 },
                { -0.422077400361521, -0.11134948984113 },
                { -0.322265008788599, 0.23632015508648 },
                { -0.12013575394419, 0.490928809797139 },
                { 0.120135753938394, 0.490928809796094 },
                { 0.322265008787236, 0.236320155085067 },
                { 0.422077400363969, -0.111349489837512 },
                { 0.38788319957867, -0.331485820278937 },
                { 0.230538823577373, -0.28441365475783 } 
            };

            Assert.IsTrue(result.IsEqual(expected, 1e-10));
        }

        [TestMethod()]
        public void TransformTest_Jagged()
        {
            double[][] sourceMatrix = new double[][]
            {
                new double[] { 2.5,  2.4 },
                new double[] { 0.5,  0.7 },
                new double[] { 2.2,  2.9 },
                new double[] { 1.9,  2.2 },
                new double[] { 3.1,  3.0 },
                new double[] { 2.3,  2.7 },
                new double[] { 2.0,  1.6 },
                new double[] { 1.0,  1.1 },
                new double[] { 1.5,  1.6 },
                new double[] { 1.1,  0.9 }
            };

            // Create a new linear kernel
            IKernel kernel = new Linear();

            // Creates the Kernel Principal Component Analysis of the given data
            var kpca = new KernelPrincipalComponentAnalysis(sourceMatrix, kernel);

            // Compute the Kernel Principal Component Analysis
            kpca.Compute();

            // The following statement throws an exception:
            // An unhandled exception of type 'System.IndexOutOfRangeException' occurred in Accord.Math.dll
            double[] actual1 = kpca.Transform(sourceMatrix[0]);

            double[][] actual = kpca.Transform(sourceMatrix);

            double[][] expected = 
            {
                new double[] { -0.827970186,  0.175115307 },
                new double[] {  1.77758033,  -0.142857227 },
                new double[] { -0.992197494, -0.384374989 },
                new double[] { -0.274210416, -0.130417207 },
                new double[] { -1.67580142,   0.209498461 },
                new double[] { -0.912949103, -0.175282444 },
                new double[] {  0.099109437,  0.349824698 },
                new double[] {  1.14457216,  -0.046417258 },
                new double[] {  0.438046137, -0.017764629 },
                new double[] {  1.22382056,   0.162675287 },
            };

            Assert.IsTrue(Matrix.IsEqual(expected[0], expected[0]));

            // Verify both are equal with 0.001 tolerance value
            Assert.IsTrue(Matrix.IsEqual(actual, expected, 0.0001));

            // Assert the result equals the transformation of the input
            double[,] result = kpca.Result;
            double[,] projection = kpca.Transform(data);
            Assert.IsTrue(Matrix.IsEqual(result, projection, 0.000001));
        }

        [TestMethod()]
        public void RevertTest()
        {
            // Using a linear kernel should be equivalent to standard PCA
            IKernel kernel = new Linear();

            // Create analysis
            KernelPrincipalComponentAnalysis target = new KernelPrincipalComponentAnalysis(data, kernel, AnalysisMethod.Center);

            // Compute
            target.Compute();

            // Compute image
            double[,] image = target.Transform(data, 2);

            // Compute pre-image
            double[,] preimage = target.Revert(image);

            // Check if pre-image equals the original data
            Assert.IsTrue(Matrix.IsEqual(data, preimage, 0.0001));
        }

        [TestMethod()]
        public void RevertTest2()
        {

            string path = @"..\..\..\..\Unit Tests\Accord.Tests.Statistics\Resources\examples.xls";

            // Create a new reader, opening a given path
            ExcelReader reader = new ExcelReader(path);

            // Afterwards, we can query the file for all
            // worksheets within the specified workbook:
            string[] sheets = reader.GetWorksheetList();

            // Finally, we can request an specific sheet:
            DataTable table = reader.GetWorksheet("Wikipedia");

            // Now, we have loaded the Excel file into a DataTable. We
            // can go further and transform it into a matrix to start
            // running other algorithms on it: 

            double[,] matrix = table.ToMatrix();

            IKernel kernel = new Gaussian(5);

            // Create analysis
            KernelPrincipalComponentAnalysis target = new KernelPrincipalComponentAnalysis(matrix,
                kernel, AnalysisMethod.Center, centerInFeatureSpace: false);

            target.Compute();

            double[,] forward = target.Result;

            double[,] reversion = target.Revert(forward);

            Assert.IsTrue(!reversion.HasNaN());
        }

        [TestMethod()]
        public void RevertTest3()
        {

            string path = @"..\..\..\..\Unit Tests\Accord.Tests.Statistics\Resources\examples.xls";

            // Create a new reader, opening a given path
            ExcelReader reader = new ExcelReader(path);

            // Afterwards, we can query the file for all
            // worksheets within the specified workbook:
            string[] sheets = reader.GetWorksheetList();

            // Finally, we can request an specific sheet:
            DataTable table = reader.GetWorksheet("Wikipedia");

            // Now, we have loaded the Excel file into a DataTable. We
            // can go further and transform it into a matrix to start
            // running other algorithms on it: 

            double[,] matrix = table.ToMatrix();

            IKernel kernel = new Polynomial(2);

            // Create analysis
            KernelPrincipalComponentAnalysis target = new KernelPrincipalComponentAnalysis(matrix,
                kernel, AnalysisMethod.Center, centerInFeatureSpace: true);

            target.Compute();

            double[,] forward = target.Result;

            double[,] reversion = target.Revert(forward);

            Assert.IsTrue(!reversion.HasNaN());
        }
    }
}
