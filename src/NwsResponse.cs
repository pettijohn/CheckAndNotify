// Generated via https://json2csharp.com/code-converters/xml-to-csharp

using System.Xml.Serialization;

[XmlRoot(ElementName="image")]
public class Image { 

	[XmlElement(ElementName="url")] 
	public string Url { get; set; } 

	[XmlElement(ElementName="title")] 
	public string Title { get; set; } 

	[XmlElement(ElementName="link")] 
	public string Link { get; set; } 
}

[XmlRoot(ElementName="current_observation")]
public class CurrentObservation { 

	[XmlElement(ElementName="credit")] 
	public string Credit { get; set; } 

	[XmlElement(ElementName="credit_URL")] 
	public string CreditURL { get; set; } 

	[XmlElement(ElementName="image")] 
	public Image Image { get; set; } 

	[XmlElement(ElementName="suggested_pickup")] 
	public string SuggestedPickup { get; set; } 

	[XmlElement(ElementName="suggested_pickup_period")] 
	public int SuggestedPickupPeriod { get; set; } 

	[XmlElement(ElementName="location")] 
	public string Location { get; set; } 

	[XmlElement(ElementName="station_id")] 
	public string StationId { get; set; } 

	[XmlElement(ElementName="latitude")] 
	public double Latitude { get; set; } 

	[XmlElement(ElementName="longitude")] 
	public double Longitude { get; set; } 

	[XmlElement(ElementName="observation_time")] 
	public string ObservationTime { get; set; } 

	[XmlElement(ElementName="observation_time_rfc822")] 
	public string ObservationTimeRfc822 { get; set; } 

	[XmlElement(ElementName="weather")] 
	public string Weather { get; set; } 

	[XmlElement(ElementName="temperature_string")] 
	public string TemperatureString { get; set; } 

	[XmlElement(ElementName="temp_f")] 
	public double TempF { get; set; } 

	[XmlElement(ElementName="temp_c")] 
	public double TempC { get; set; } 

	[XmlElement(ElementName="relative_humidity")] 
	public int RelativeHumidity { get; set; } 

	[XmlElement(ElementName="wind_string")] 
	public string WindString { get; set; } 

	[XmlElement(ElementName="wind_dir")] 
	public string WindDir { get; set; } 

	[XmlElement(ElementName="wind_degrees")] 
	public int WindDegrees { get; set; } 

	[XmlElement(ElementName="wind_mph")] 
	public double WindMph { get; set; } 

	[XmlElement(ElementName="wind_kt")] 
	public int WindKt { get; set; } 

	[XmlElement(ElementName="pressure_string")] 
	public string PressureString { get; set; } 

	[XmlElement(ElementName="pressure_mb")] 
	public double PressureMb { get; set; } 

	[XmlElement(ElementName="pressure_in")] 
	public double PressureIn { get; set; } 

	[XmlElement(ElementName="dewpoint_string")] 
	public string DewpointString { get; set; } 

	[XmlElement(ElementName="dewpoint_f")] 
	public double DewpointF { get; set; } 

	[XmlElement(ElementName="dewpoint_c")] 
	public double DewpointC { get; set; } 

	[XmlElement(ElementName="visibility_mi")] 
	public double VisibilityMi { get; set; } 

	[XmlElement(ElementName="icon_url_base")] 
	public string IconUrlBase { get; set; } 

	[XmlElement(ElementName="two_day_history_url")] 
	public string TwoDayHistoryUrl { get; set; } 

	[XmlElement(ElementName="icon_url_name")] 
	public string IconUrlName { get; set; } 

	[XmlElement(ElementName="ob_url")] 
	public string ObUrl { get; set; } 

	[XmlElement(ElementName="disclaimer_url")] 
	public string DisclaimerUrl { get; set; } 

	[XmlElement(ElementName="copyright_url")] 
	public string CopyrightUrl { get; set; } 

	[XmlElement(ElementName="privacy_policy_url")] 
	public string PrivacyPolicyUrl { get; set; } 

	[XmlAttribute(AttributeName="version")] 
	public double Version { get; set; } 

	[XmlAttribute(AttributeName="xsd")] 
	public string Xsd { get; set; } 

	[XmlAttribute(AttributeName="xsi")] 
	public string Xsi { get; set; } 

	[XmlAttribute(AttributeName="noNamespaceSchemaLocation")] 
	public string NoNamespaceSchemaLocation { get; set; } 

	[XmlText] 
	public string Text { get; set; } 
}

