#include "gdal_priv.h"
#include "gdal_frmts.h"
#include "ogrsf_frmts.h" 
#include <iostream>
using namespace std;
int main()
{
	GDALDataset *poDataSet;
	char *file = "px.dxf";
	
	GDALAllRegister(); 
	OGRRegisterAll();
	GDALRegister_AIGrid();
	CPLSetConfigOption("GDAL_FILENAME_IS_UTF8", "NO");
	GDALOpenInfo openinfo(file, GDAL_OF_UPDATE); 
	GDALDriverManager dm; GDALDriver d;
	dm.AutoLoadDrivers(); 
	cout<<d.GetDescription();
	//char **papszOptions = (char **)CPLCalloc(sizeof(char *), 3);
	//papszOptions[0] = "HEADER=header.dxf";
	//papszOptions[1] = "TRAILER=trailer.dxf";
	
	//GDALDataset* poData=d.Create("new.dxf", 0, 0, 0, GDT_Unknown, papszOptions);
	//poData->CreateLayer("test");
	cout << dm.GetDriverCount();
	for (int i = 0; i < GDALGetDriverCount();i++)
	{
		cout << ((GDALDriver*)GDALGetDriver(i))->GetDescription() << ",";;
	}
	poDataSet = (decltype(poDataSet))GDALOpenEx(file, GDAL_OF_UPDATE, GDAL_OF_ALL,nullptr,nullptr);
	if (nullptr==poDataSet)
	{
		cout << "打开文件失败!" << endl;
		return 0;
	}
	int pnum = 0, lnum = 0;

	char **papszOptions = (char **)CPLCalloc(sizeof(char *), 3);
	papszOptions[0] = "HEADER=header.dxf";
	papszOptions[1] = "TRAILER=trailer.dxf";
	poDataSet = d.Create("out.dxf",0,0,0,GDT_Unknown, papszOptions);
	
	CPLFree(papszOptions);
	if (poDataSet == NULL)
	{
		printf("Creation of output file failed.\n");
		exit(1);
	}
	OGRLayer *poLayer;

	poLayer = poDataSet->CreateLayer("hatch_out", NULL, wkbUnknown, NULL);
	if (poLayer == NULL)
	{
		printf("Layer creation failed.\n");
		exit(1);
	}

	OGRFeature *poFeature = OGRFeature::CreateFeature(poLayer->GetLayerDefn());

	// 生成一个正方形,放置在"正方形"图层  
	OGRLinearRing oSquare;
	oSquare.addPoint(0.0, 0.0);
	oSquare.addPoint(1.0, 0.0);
	oSquare.addPoint(1.0, 1.0);
	oSquare.addPoint(0.0, 1.0);
	oSquare.closeRings();
	poFeature->SetField("Layer", "正方形");
	poFeature->SetGeometry(&oSquare);
	if (poLayer->CreateFeature(poFeature) != OGRERR_NONE)
	{
		printf("Failed to create feature in dxffile.\n");
		exit(1);
	}

	// 生成一个三角形,放置在"三角形"图层  
	poFeature = OGRFeature::CreateFeature(poLayer->GetLayerDefn());
	OGRLinearRing oTriangle;
	oTriangle.addPoint(2.0, 2.0);
	oTriangle.addPoint(3.0, 2.0);
	oTriangle.addPoint(3.0, 3.0);
	oTriangle.closeRings();
	poFeature->SetField("Layer", "三角形");
	poFeature->SetGeometry(&oTriangle);
	if (poLayer->CreateFeature(poFeature) != OGRERR_NONE)
	{
		printf("Failed to create feature in dxffile.\n");
		exit(1);
	}

	// 生成一个菱形,放置在"三角形"图层  
	poFeature = OGRFeature::CreateFeature(poLayer->GetLayerDefn());
	OGRLinearRing oRhombus;
	oRhombus.addPoint(4.0, 3.0);
	oRhombus.addPoint(5.0, 4.0);
	oRhombus.addPoint(4.0, 5.0);
	oRhombus.addPoint(3.0, 4.0);
	oRhombus.closeRings();
	poFeature->SetField("Layer", "三角形");
	poFeature->SetGeometry(&oRhombus);
	if (poLayer->CreateFeature(poFeature) != OGRERR_NONE)
	{
		printf("Failed to create feature in dxffile.\n");
		exit(1);
	}

	// 生成一个多段线,不设置图层，将默认层  
	poFeature = OGRFeature::CreateFeature(poLayer->GetLayerDefn());
	OGRLinearRing oLinearRing;
	oLinearRing.addPoint(5.0, 5.0);
	oLinearRing.addPoint(6.0, 4.0);
	oLinearRing.addPoint(7.0, 5.0);
	oLinearRing.addPoint(8.0, 4.0);
	poFeature->SetGeometry(&oLinearRing);
	if (poLayer->CreateFeature(poFeature) != OGRERR_NONE)
	{
		printf("Failed to create feature in dxffile.\n");
		exit(1);
	}

	OGRFeature::DestroyFeature(poFeature);

	poDataSet->Release();

	for (int i = 0; i < poDataSet->GetLayerCount();i++)
	{
		cout << "第" << i + 1 << "层" << endl;
		OGRLayer* poLayer = poDataSet->GetLayer(i);
		OGRFeature *poFeature;

		poFeature = OGRFeature::CreateFeature(poLayer->GetLayerDefn());
		OGRLinearRing oSquare;
		oSquare.addPoint(0.0, 0.0);
		oSquare.addPoint(1.0, 0.0);
		oSquare.addPoint(1.0, 1.0);
		oSquare.addPoint(0.0, 1.0);
		oSquare.closeRings();
		poFeature->SetField("Layer", "正方形");
		poFeature->SetGeometry(&oSquare);
		if (poLayer->CreateFeature(poFeature) != OGRERR_NONE)
		{
			printf("Failed to create feature in dxffile.\n");
			//exit(1);
		}
		while ((poFeature = poLayer->GetNextFeature()) != NULL)
		{

			//poFeature->SetGeometry(NULL);
			OGRGeometry *poGeometry = poFeature->GetGeometryRef();
			if (poGeometry != nullptr&&wkbFlatten(poGeometry->getGeometryType()) == wkbPolygon)
			{
				cout << "第" << i << "个图层" << "第" << pnum++ << "个面" << endl;

				OGRPolygon *polygon = (OGRPolygon*)poGeometry;

				OGRLinearRing *poLine = polygon->getExteriorRing();
				// 得到线串点的数目
				for (int j = 0; j < poLine->getNumPoints(); j++)
				{
					//然后读取每个顶点
					OGRPoint point;
					poLine->getPoint(j, &point);
					//这样每个顶点都就能被读取进来；
					cout << point.getX() << "," << point.getY() << "," << point.getZ() << endl;
				}
			}
			if (poGeometry != nullptr&&wkbFlatten(poGeometry->getGeometryType()) == wkbLineString)
			{
				OGRLineString *ogrLineString = (OGRLineString *)poGeometry;
				int nPointNum = ogrLineString->getNumPoints();
				for (int j = 0; j < nPointNum; j++)
				{
					OGRPoint point;
					ogrLineString->getPoint(j, &point);
					ogrLineString->setPoint(j, 1, 2, 3);
					//这样每个顶点都就能被读取进来；
					cout << point.getX() << "," << point.getY() << "," << point.getZ() << endl;
				}
			}
			poLayer->SetFeature(poFeature);
		}
		OGRFeature::DestroyFeature(poFeature);
		
	}
	cout << poDataSet->GetFileList();
	//GDALCopyDatasetFiles(poDataSet, "tst2.dxf", "test.tif");
	poDataSet->Release();
	
	cout << "over" << endl;
	system("pause");
	return 0;
}