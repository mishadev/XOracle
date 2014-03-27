<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="XOracle.Azure" generation="1" functional="0" release="0" Id="26a42769-143d-46da-90f3-3e1c1745b542" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="XOracle.AzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="XOracle.Azure.Web.Front:httpIn" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/XOracle.Azure/XOracle.AzureGroup/LB:XOracle.Azure.Web.Front:httpIn" />
          </inToChannel>
        </inPort>
        <inPort name="XOracle.Azure.Web.Front:HttpsIn" protocol="https">
          <inToChannel>
            <lBChannelMoniker name="/XOracle.Azure/XOracle.AzureGroup/LB:XOracle.Azure.Web.Front:HttpsIn" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|XOracle.Azure.Web.Front:localhost" defaultValue="">
          <maps>
            <mapMoniker name="/XOracle.Azure/XOracle.AzureGroup/MapCertificate|XOracle.Azure.Web.Front:localhost" />
          </maps>
        </aCS>
        <aCS name="XOracle.Azure.Web.Front:DataConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/XOracle.Azure/XOracle.AzureGroup/MapXOracle.Azure.Web.Front:DataConnectionString" />
          </maps>
        </aCS>
        <aCS name="XOracle.Azure.Web.Front:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/XOracle.Azure/XOracle.AzureGroup/MapXOracle.Azure.Web.Front:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="XOracle.Azure.Web.FrontInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/XOracle.Azure/XOracle.AzureGroup/MapXOracle.Azure.Web.FrontInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:XOracle.Azure.Web.Front:httpIn">
          <toPorts>
            <inPortMoniker name="/XOracle.Azure/XOracle.AzureGroup/XOracle.Azure.Web.Front/httpIn" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:XOracle.Azure.Web.Front:HttpsIn">
          <toPorts>
            <inPortMoniker name="/XOracle.Azure/XOracle.AzureGroup/XOracle.Azure.Web.Front/HttpsIn" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapCertificate|XOracle.Azure.Web.Front:localhost" kind="Identity">
          <certificate>
            <certificateMoniker name="/XOracle.Azure/XOracle.AzureGroup/XOracle.Azure.Web.Front/localhost" />
          </certificate>
        </map>
        <map name="MapXOracle.Azure.Web.Front:DataConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/XOracle.Azure/XOracle.AzureGroup/XOracle.Azure.Web.Front/DataConnectionString" />
          </setting>
        </map>
        <map name="MapXOracle.Azure.Web.Front:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/XOracle.Azure/XOracle.AzureGroup/XOracle.Azure.Web.Front/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapXOracle.Azure.Web.FrontInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/XOracle.Azure/XOracle.AzureGroup/XOracle.Azure.Web.FrontInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="XOracle.Azure.Web.Front" generation="1" functional="0" release="0" software="D:\XOracle\XOracle\XOracle.Azure\csx\Debug\roles\XOracle.Azure.Web.Front" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="httpIn" protocol="http" portRanges="80" />
              <inPort name="HttpsIn" protocol="https" portRanges="446">
                <certificate>
                  <certificateMoniker name="/XOracle.Azure/XOracle.AzureGroup/XOracle.Azure.Web.Front/localhost" />
                </certificate>
              </inPort>
            </componentports>
            <settings>
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;XOracle.Azure.Web.Front&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;XOracle.Azure.Web.Front&quot;&gt;&lt;e name=&quot;httpIn&quot; /&gt;&lt;e name=&quot;HttpsIn&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0localhost" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/XOracle.Azure/XOracle.AzureGroup/XOracle.Azure.Web.Front/localhost" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="localhost" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/XOracle.Azure/XOracle.AzureGroup/XOracle.Azure.Web.FrontInstances" />
            <sCSPolicyUpdateDomainMoniker name="/XOracle.Azure/XOracle.AzureGroup/XOracle.Azure.Web.FrontUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/XOracle.Azure/XOracle.AzureGroup/XOracle.Azure.Web.FrontFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="XOracle.Azure.Web.FrontUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="XOracle.Azure.Web.FrontFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="XOracle.Azure.Web.FrontInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="21564f18-4120-4127-8398-51ea35e00e00" ref="Microsoft.RedDog.Contract\ServiceContract\XOracle.AzureContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="747a18e1-db5d-4975-8716-078989cffee1" ref="Microsoft.RedDog.Contract\Interface\XOracle.Azure.Web.Front:httpIn@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/XOracle.Azure/XOracle.AzureGroup/XOracle.Azure.Web.Front:httpIn" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="a89dc22c-b075-4e53-a135-2599597f0508" ref="Microsoft.RedDog.Contract\Interface\XOracle.Azure.Web.Front:HttpsIn@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/XOracle.Azure/XOracle.AzureGroup/XOracle.Azure.Web.Front:HttpsIn" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>