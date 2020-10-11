﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Google.Cloud.BigQuery.V2;
using Google.Apis.Auth;
using UnityEditor.PackageManager;
using System.Data.SqlTypes;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;

/*
SELECT
  date,
  country_code,
  subregion1_name,
  location_geometry,
  cumulative_confirmed,
  cumulative_deceased,
  new_confirmed,
  new_deceased
FROM
  bigquery-public -data.covid19_open_data.covid19_open_data
WHERE
  date = "2020-10-01"
  AND subregion2_code IS NULL
  AND country_code = "US"
*/

/*
 * SELECT
  cumulative_confirmed,
  subregion1_name,
  subregion2_name,
FROM
  bigquery-public-data.covid19_open_data.covid19_open_data
WHERE
  date = "2020-10-05"
AND
  country_code = "US"
AND
  subregion1_code = "AZ"
 */




public class DataGrabber
{
    private String projectCode,                            //code to access the project, also called project key
                   source,                                 //what we are using from googles query
                   table,                                  //table we are getting from
                   subtable;                               //subtable that we need to grab the data from within our table
    private String[] select = new String[10];              //commas to separate selects
    private String[] where = new String[10];               //if more than one where, AND to separate each where



    //constructor
    public DataGrabber(String projectCode, String source, String table, String subtable)
    {
        this.projectCode = projectCode;
        this.source = source;
        this.table = table;
        this.subtable = subtable;
        //initialize arrays to null
        for(int i = 0; i < 10; i++)
        {
            this.select[i] = "";
        }
        for (int i = 0; i < 10; i++) 
        {
            this.where[i] = "";
        }
    }

    //add data that we want to receive
    public bool AddSelect(String select)
    {
        for(int i = 0; i < 10; i++)
        {
            if(this.select[i].CompareTo("") == 0) // look for first empty value
            {
                this.select[i] = select;
                return true; //successful select parameter added
            }
        }
        return false; //parameter not added
    }
    
    //add data parameters from what we want to specify
    public bool AddWhere(String where)
    {
        for(int i = 0; i < 10; i++)
        {
            if(this.where[i].CompareTo("") == 0) // look for first empty value
            {
                this.where[i] = where;
                return true; //successful select parameter added
            }
        }
        return false; //parameter not added
    }

    //forms the query
    public String FormQuery()
    {
        String query = "";

        query += "SELECT\n";
        if(this.select[0].CompareTo("") != 0)
        {
            query += this.select[0];
            for(int i = 1; i < 10; i++)  //start at index 1, b/c 0 was filled
            {
                query += ", \n" + this.select[i];
            }
            query += "\n";
        }

        query += "FROM\n";
        query += this.source + "." + this.table + "." + this.subtable;
        query += "\n";

        query += "WHERE\n";
        if(this.where[0].CompareTo("") != 0)
        {
            query += where[0];
            for(int i = 1; i < 10; i++)     //start at index 1, b/c 0 was filled
            {
                query += "\nAND\n";
                query += this.where[i];
            }
        }

        return query;
    }

    public String GetData()
    {
        var ourClient = BigQueryClient.Create("direct-volt-292119");
        String query = this.FormQuery();

        var results = ourClient.ExecuteQuery(query, parameters: null);

        return results;


    }









}
