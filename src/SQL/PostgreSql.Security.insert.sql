-- Roles
INSERT INTO public.stormag(primarykey, name, isuser, isgroup, isrole, enabled)
VALUES ('b02d3e4a-ce84-40d7-8d27-b5d85f543a84', 'AllAccess', false, false, true, true);

INSERT INTO public.stormag(primarykey, name, isuser, isgroup, isrole, enabled)
VALUES ('c009f57d-2237-4df6-b121-c003c1694056', 'NoAccess', false, false, true, true);

-- AllAccess -> IIS.AsyncOpenXmlReportsSample.Brand
INSERT INTO public.storms(primarykey, name, isattribute, isoperation, isview, isclass, sharedoper)
VALUES ('099b59ee-2aa1-44e9-9e7e-3634e3523357', 'IIS.AsyncOpenXmlReportsSample.Brand', false, false, false, true, false);

INSERT INTO public.stormp(primarykey, subject_m0, agent_m0)
VALUES ('c9def3e9-5b10-47f6-adc1-ff39567f8638', '099b59ee-2aa1-44e9-9e7e-3634e3523357', 'b02d3e4a-ce84-40d7-8d27-b5d85f543a84');

INSERT INTO public.stormac(primarykey, typeaccess, permition_m0)
VALUES ('ea42b57f-d5db-4f0e-9ccb-c05c81d71b3c', 'Full', 'c9def3e9-5b10-47f6-adc1-ff39567f8638');

-- AllAccess -> IIS.AsyncOpenXmlReportsSample.Car
INSERT INTO public.storms(primarykey, name, isattribute, isoperation, isview, isclass, sharedoper)
VALUES ('8bbd776b-c359-4878-9048-d2198996e1fa', 'IIS.AsyncOpenXmlReportsSample.Car', false, false, false, true, false);

INSERT INTO public.stormp(primarykey, subject_m0, agent_m0)
VALUES ('eb8a4ff2-63f7-402a-b131-9fdf8d0d11a8', '8bbd776b-c359-4878-9048-d2198996e1fa', 'b02d3e4a-ce84-40d7-8d27-b5d85f543a84');

INSERT INTO public.stormac(primarykey, typeaccess, permition_m0)
VALUES ('0727ef01-2616-4700-9db2-709cd57fae3b', 'Full', 'eb8a4ff2-63f7-402a-b131-9fdf8d0d11a8');

-- AllAccess -> IIS.AsyncOpenXmlReportsSample.ProducingCountry
INSERT INTO public.storms(primarykey, name, isattribute, isoperation, isview, isclass, sharedoper)
VALUES ('a01855a2-372f-48d8-8175-b37c550cf26c', 'IIS.AsyncOpenXmlReportsSample.ProducingCountry', false, false, false, true, false);

INSERT INTO public.stormp(primarykey, subject_m0, agent_m0)
VALUES ('bdd0abe9-24ac-4779-acc3-d3d991a917e5', 'a01855a2-372f-48d8-8175-b37c550cf26c', 'b02d3e4a-ce84-40d7-8d27-b5d85f543a84');

INSERT INTO public.stormac(primarykey, typeaccess, permition_m0)
VALUES ('3e587e5f-00e7-4e7d-9106-2b7552a0e4e4', 'Full', 'bdd0abe9-24ac-4779-acc3-d3d991a917e5');

-- AllAccess -> IIS.AsyncOpenXmlReportsSample.SparePart
INSERT INTO public.storms(primarykey, name, isattribute, isoperation, isview, isclass, sharedoper)
VALUES ('9fe6d51b-acc9-4884-8fe4-24323d398664', 'IIS.AsyncOpenXmlReportsSample.SparePart', false, false, false, true, false);

INSERT INTO public.stormp(primarykey, subject_m0, agent_m0)
VALUES ('5737c9ca-aadf-4e83-9bb1-833f12b13fd7', '9fe6d51b-acc9-4884-8fe4-24323d398664', 'b02d3e4a-ce84-40d7-8d27-b5d85f543a84');

INSERT INTO public.stormac(primarykey, typeaccess, permition_m0)
VALUES ('dd827b6e-bb1b-4bf5-9bd7-fce5a8ba093e', 'Full', '5737c9ca-aadf-4e83-9bb1-833f12b13fd7');

-- AllAccess -> Reports.SampleReport
INSERT INTO public.storms(primarykey, name, isattribute, isoperation, isview, isclass, sharedoper)
VALUES ('e1403cb5-0726-44e9-adf3-d2d5a92151ee', 'Reports.SampleReport', false, true, false, false, false);

INSERT INTO public.stormp(primarykey, subject_m0, agent_m0)
VALUES ('ae1e9ff4-65a1-407a-a82d-6225abb03a81', 'e1403cb5-0726-44e9-adf3-d2d5a92151ee', 'b02d3e4a-ce84-40d7-8d27-b5d85f543a84');

INSERT INTO public.stormac(primarykey, typeaccess, permition_m0)
VALUES ('fec68377-0499-4a3e-87bd-e1d26adc9c9a', 'Full', 'ae1e9ff4-65a1-407a-a82d-6225abb03a81');

-- AllAccess -> Reports.CarListReport
INSERT INTO public.storms(primarykey, name, isattribute, isoperation, isview, isclass, sharedoper)
VALUES ('141b3e42-2070-4d8a-a109-159a921b1aa2', 'Reports.CarListReport', false, true, false, false, false);

INSERT INTO public.stormp(primarykey, subject_m0, agent_m0)
VALUES ('3646c92b-3c86-4413-894e-675bfb2f2e35', '141b3e42-2070-4d8a-a109-159a921b1aa2', 'b02d3e4a-ce84-40d7-8d27-b5d85f543a84');

INSERT INTO public.stormac(primarykey, typeaccess, permition_m0)
VALUES ('d2f18840-625d-4255-a345-73bf6f863bbc', 'Full', '3646c92b-3c86-4413-894e-675bfb2f2e35');
