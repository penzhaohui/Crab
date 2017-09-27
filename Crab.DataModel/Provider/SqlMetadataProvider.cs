using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using Crab.DataModel.Utility;
using Crab.DataModel;

namespace Crab.DataModel.Provider
{
    /// <summary>
    /// Manages storage of datamodel information in a
    ///     SQL Server database.
    /// </summary>
    public class SqlMetadataProvider: MetadataProvider
    {
        #region private fields
        private string _sqlConnectionString;
        #endregion

        public override string ConnectionString
        {
            get { return _sqlConnectionString; }
        }

        /// <summary>
        /// Initialize the provider with the values from configuration
        /// </summary>
        /// <param name="name">The name of the provider</param>
        /// <param name="config">The configuration collection</param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (string.IsNullOrEmpty(name))
            {
                name = "SqlDataModelProvider";
            }
            base.Initialize(name, config);
            string connectionStringName = config["connectionStringName"];
            if(string.IsNullOrEmpty(connectionStringName))
            {
                throw new ProviderException("Connection_name_not_specified");
            }
            this._sqlConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        public override DataNode GetDataNode(Guid id)
        {
            using (SqlDataReader dr = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, "select * from datanodes where Id = @Id",
                new SqlParameter("@Id", id)))
            {
                if (dr.Read())
                {
                    int nodeType = (int)dr["NodeType"];
                    DataNode node = NewDataNode(nodeType);
                    SqlEntityHelper<DataNode>.WriteEntity(node, dr);
                    return node;
                }
            }
            return null;
        }

        public override DataNode[] GetAllNodes(int nodeType)
        {
            List<DataNode> nodes = new List<DataNode>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@NodeType", nodeType),
                new SqlParameter("@TenantId", DataModelContext.TenantId),
            };
            using (SqlDataReader dr = SqlHelper.ExecuteReader(ConnectionString, 
                CommandType.Text, 
                "select * from datanodes where NodeType = @NodeType and (TenantId is NULL or TenantId=@TenantId)",
                parameters))
            {
                while(dr.Read())
                {
                    DataNode node = NewDataNode(nodeType);
                    SqlEntityHelper<DataNode>.WriteEntity(node, dr);
                    nodes.Add(node);
                }
            }
            return nodes.ToArray();
        }

        public override DataNode[] GetChildNodes(Guid parentId)
        {
            List<DataNode> nodes = new List<DataNode>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@ParentId", parentId),
                new SqlParameter("@TenantId", DataModelContext.TenantId),
            };
            using (SqlDataReader dr = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
                "select * from datanodes where ParentId = @ParentId and (TenantId is NULL or TenantId=@TenantId)",
                parameters))
            {
                while (dr.Read())
                {
                    int nodeType = (int)dr["NodeType"];
                    DataNode node = NewDataNode(nodeType);
                    SqlEntityHelper<DataNode>.WriteEntity(node, dr);
                    nodes.Add(node);
                }
            }
            return nodes.ToArray();
        }

        /// <summary>
        /// Get all properties of a node
        /// </summary>
        /// <param name="tenantId">The uniqueidentifier of a tenant</param>
        /// <param name="nodeId">The uniqueidentifier of the node</param>
        /// <returns></returns>
        public override DataProperty[] GetProperties(Guid nodeId)
        {
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@TenantId", DataModelContext.TenantId),
                new SqlParameter("@ParentId", nodeId),
            };

            List<DataProperty> properties = new List<DataProperty>();
            DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, 
                CommandType.StoredProcedure, 
                "crab_GetDataProperties",
                parameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dataRow in ds.Tables[0].Rows)
                {
                    DataProperty property = new DataProperty();
                    SqlEntityHelper<DataProperty>.WriteEntity(property, dataRow);
                    properties.Add(property);
                }
            }
            return properties.ToArray();
        }

        /// <summary>
        /// Create a new data node in database
        /// </summary>
        /// <param name="node">The node object to create</param>
        /// <returns>The DataNode instance created. null if no node is created in database</returns>
        public override DataNode CreateDataNode(DataNode node)
        {
            if(node == null)
                throw new ArgumentNullException("node");
            if(string.IsNullOrEmpty(node.Name))
                throw new ArgumentNullException("node.Name");
            if (!CheckUnique(node.ParentId, node.Name))
                throw new ArgumentException("node.Name");
            if (node.Id == Guid.Empty)
                node.Id = Guid.NewGuid();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(SqlEntityHelper<DataNode>.GenerateInsertScripts(node));
            foreach (DataProperty dataProperty in node.Properties)
                sb.AppendLine(SqlEntityHelper<DataProperty>.GenerateInsertScripts(dataProperty));
            string batchScripts = sb.ToString();
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, batchScripts);
            node.LoadChildNodes();
            return node;
        }

        /// <summary>
        /// Update the data node to database
        /// </summary>
        /// <param name="node">The DataNode object to be updated</param>
        public override void UpdateDataNode(DataNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");
            StringBuilder sb = new StringBuilder();
            if (node.TenantId == DataModelContext.TenantId) //only the node owner could update the node value
            {
                sb.AppendLine(SqlEntityHelper<DataNode>.GenerateUpdateScripts(node));
            }
            DataProperty[] inserted;
            DataProperty[] updated;
            DataProperty[] deleted;
            node.Properties.GetAllChanges(out inserted, out updated, out deleted);
            foreach (DataProperty dataProperty in inserted)
                sb.AppendLine(SqlEntityHelper<DataProperty>.GenerateInsertScripts(dataProperty));
            foreach (DataProperty dataProperty in updated)
                sb.AppendLine(SqlEntityHelper<DataProperty>.GenerateUpdateScripts(dataProperty));
            foreach (DataProperty dataProperty in deleted)
                sb.AppendLine(SqlEntityHelper<DataProperty>.GenerateDeleteScripts(dataProperty));
            string batchScripts = sb.ToString();
            if (!string.IsNullOrEmpty(batchScripts))
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, batchScripts);
                if (inserted.Length > 0 || updated.Length > 0 || deleted.Length > 0)
                    node.Properties.RefreshCollection();
            }
        }

        /// <summary>
        /// Delete the node from database and the parent child nodes collection
        /// </summary>
        /// <param name="node">The DataNode object to be deleted</param>
        public override void DeleteDataNode(DataNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(SqlEntityHelper<DataNode>.GenerateDeleteScripts(node));
            //The properties are deleted cascadely by the database
            sb.Append("DELETE FROM DataNodes WHERE ParentId NOT IN (SELECT ID FROM DataNodes)");
            string batchScripts = sb.ToString();
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, batchScripts);
            /*if (node.ParentNode != null)
            {
                node.ParentNode.ChildNodes.Remove(node.Id);
            }*/
        }

        private bool CheckUnique(Guid? parentId, string nodeName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT COUNT(*) FROM DataNodes WHERE Name=@Name");
            if (parentId == null)
                sb.Append(" AND ParentId IS NULL");
            else
                sb.Append(" AND ParentId=@ParentId");
            sb.Append(" AND TenantId=@TenantId");
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@Name", nodeName));
            if(parentId!=null)
                sqlParams.Add(new SqlParameter("@ParentId", (Guid)parentId));
            sqlParams.Add(new SqlParameter("@TenantId", DataModelContext.TenantId));
            int count = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, sb.ToString(), sqlParams.ToArray());
            return count == 0;
        }
    }
}
