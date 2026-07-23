const NODES = [{"name": "Velia", "x": 10069.5, "y": 74166.3, "type": "City"}, {"name": "Western Guard Camp", "x": -59267.3, "y": 39882.5, "type": "Town"}, {"name": "Cron Castle", "x": 21308.4, "y": 129829.0, "type": "Dangerous"}, {"name": "Western Gateway", "x": -81151.1, "y": 51561.5, "type": "Gateway"}, {"name": "Bandit's Den Byway", "x": -66134.5, "y": -3042.74, "type": "Gateway"}, {"name": "Heidel Pass", "x": 42741.6, "y": 28601.4, "type": "Gateway"}, {"name": "Bartali Farm", "x": 11539.6, "y": 56002.8, "type": "Trading Post"}, {"name": "Finto Farm", "x": 38597.7, "y": 79789.8, "type": "Trading Post"}, {"name": "Goblin Cave", "x": 59168.7, "y": 40867.6, "type": "Dangerous"}, {"name": "Ancient Stone Chamber", "x": -45893.4, "y": 3428.7, "type": "Connection"}, {"name": "Imp Cave", "x": -33969.7, "y": 58889.8, "type": "Connection"}, {"name": "Loggia Farm", "x": -12868.5, "y": 75713.5, "type": "Trading Post"}, {"name": "Marino Farm", "x": -2374.41, "y": 47285.8, "type": "Trading Post"}, {"name": "Cron Castle Site", "x": 33925.1, "y": 116931.0, "type": "Connection"}, {"name": "Ehwaz Hill", "x": 60108.4, "y": 91693.1, "type": "Connection"}, {"name": "Forest of Plunder", "x": 41447.0, "y": 62739.1, "type": "Connection"}, {"name": "Balenos Forest", "x": 30804.1, "y": 46707.9, "type": "Trading Post"}, {"name": "Toscani Farm", "x": -29592.9, "y": 26244.2, "type": "Trading Post"}, {"name": "Coastal Cave", "x": -14406.0, "y": 89483.3, "type": "Connection"}, {"name": "Altar of Agris", "x": -58885.7, "y": 67316.1, "type": "Connection"}, {"name": "Forest of Seclusion", "x": -48426.3, "y": 21206.2, "type": "Dangerous"}, {"name": "Coastal Cliff", "x": -59269.4, "y": 90509.9, "type": "Connection"}, {"name": "Olvia", "x": -142268.0, "y": 126063.0, "type": "Town"}, {"name": "Florin Gateway", "x": -199352.0, "y": 133207.0, "type": "Connection"}, {"name": "Elder's Bridge", "x": -264092.0, "y": 111295.0, "type": "Connection"}, {"name": "Casta Farm", "x": -123315.0, "y": 117438.0, "type": "Connection"}, {"name": "Wale Farm", "x": -154855.0, "y": 134961.0, "type": "Connection"}, {"name": "Wolf Hills", "x": -114132.0, "y": 89069.4, "type": "Connection"}, {"name": "Balenos River Mouth", "x": -85264.9, "y": 106768.0, "type": "Connection"}, {"name": "Terrmian Cliff", "x": -182714.0, "y": 140064.0, "type": "Connection"}, {"name": "Foot of Terrmian Mountain", "x": -271541.0, "y": 138525.0, "type": "Connection"}, {"name": "Olvia Coast", "x": -94187.7, "y": 139122.0, "type": "Connection"}, {"name": "Epheria Ridge", "x": -338001.0, "y": 77097.1, "type": "Connection"}, {"name": "Mask Owl's Forest", "x": -199797.0, "y": 96972.0, "type": "Connection"}, {"name": "Santo Manzi Investment Bank", "x": 12596.6, "y": 74355.4, "type": ""}, {"name": "Bahar Investment Bank", "x": 21945.1, "y": 76335.0, "type": ""}, {"name": "Specialties", "x": 15570.6, "y": 55581.5, "type": ""}, {"name": "Specialties", "x": 40237.5, "y": 79874.7, "type": ""}, {"name": "Specialties", "x": 1591.4, "y": 47420.8, "type": ""}, {"name": "Specialties", "x": -29480.7, "y": 29403.7, "type": ""}, {"name": "Specialties", "x": -122533.0, "y": 115138.0, "type": ""}, {"name": "Specialties", "x": -143154.0, "y": 140347.0, "type": ""}, {"name": "Specialties", "x": 158123.0, "y": 293411.0, "type": ""}, {"name": "Potato Farming", "x": 12081.1, "y": 61492.8, "type": ""}, {"name": "Chicken Meat Production", "x": 14966.6, "y": 56009.4, "type": ""}, {"name": "Ossuary", "x": 11975.2, "y": 57181.8, "type": ""}, {"name": "Ossuary", "x": 12040.6, "y": 56698.5, "type": ""}, {"name": "Potato Farming", "x": 35830.6, "y": 83866.7, "type": ""}, {"name": "Chicken Meat Production", "x": 34555.1, "y": 79284.3, "type": ""}, {"name": "Mining", "x": 58794.3, "y": 46326.4, "type": ""}, {"name": "Lumbering", "x": 59190.9, "y": 45424.3, "type": ""}, {"name": "Excavation", "x": -40091.3, "y": 3157.71, "type": ""}, {"name": "Mining", "x": -34761.2, "y": 63438.7, "type": ""}, {"name": "Mining", "x": -34365.5, "y": 63534.9, "type": ""}, {"name": "Potato Farming", "x": -9097.01, "y": 71905.9, "type": ""}, {"name": "Specialties", "x": -8413.7, "y": 73556.2, "type": ""}, {"name": "Gathering", "x": 31076.8, "y": 119793.0, "type": ""}, {"name": "Mining", "x": 30958.4, "y": 119621.0, "type": ""}, {"name": "Gathering", "x": 57329.2, "y": 90883.0, "type": ""}, {"name": "Lumbering", "x": 57916.7, "y": 91132.5, "type": ""}, {"name": "Gathering", "x": 42482.6, "y": 62665.1, "type": ""}, {"name": "Lumbering", "x": 29177.4, "y": 42041.6, "type": ""}, {"name": "Gathering", "x": 29975.1, "y": 43421.2, "type": ""}, {"name": "Corn Farming", "x": -33780.4, "y": 29837.0, "type": ""}, {"name": "Corn Farming", "x": -33752.9, "y": 31141.8, "type": ""}, {"name": "Mining", "x": -32743.3, "y": 88236.2, "type": ""}, {"name": "Mining", "x": -33302.7, "y": 87924.3, "type": ""}, {"name": "Gathering", "x": -53262.1, "y": 83058.2, "type": ""}, {"name": "Lumbering", "x": -48651.7, "y": 23488.6, "type": ""}, {"name": "Mining", "x": -48934.7, "y": 23120.6, "type": ""}, {"name": "Gathering", "x": -58589.6, "y": 98236.1, "type": ""}, {"name": "Mining", "x": -56646.2, "y": 98159.5, "type": ""}, {"name": "Lumbering", "x": -126109.0, "y": 103013.0, "type": ""}, {"name": "Mining", "x": -169985.0, "y": 163784.0, "type": ""}, {"name": "Grapes", "x": -128664.0, "y": 115623.0, "type": ""}, {"name": "Olives", "x": -159371.0, "y": 137663.0, "type": ""}, {"name": "Specialties", "x": -138210.0, "y": 125827.0, "type": ""}, {"name": "Heidel", "x": 35842.0, "y": -35802.3, "type": "City"}, {"name": "Glish", "x": -17080.0, "y": -120955.0, "type": "Town"}, {"name": "Northern Guard Camp", "x": 38842.6, "y": -12015.8, "type": "Gateway"}, {"name": "Central Guard Camp", "x": 21646.5, "y": -90455.8, "type": "Gateway"}, {"name": "Southern Guard Camp", "x": 38193.2, "y": -136414.0, "type": "Gateway"}, {"name": "Northwestern Gateway", "x": -37861.2, "y": -79541.4, "type": "Gateway"}, {"name": "Southwestern Gateway", "x": -43930.7, "y": -130665.0, "type": "Gateway"}, {"name": "Eastern Border", "x": 100047.0, "y": -40753.6, "type": "Gateway"}, {"name": "Eastern Gateway", "x": 68123.9, "y": -85541.9, "type": "Gateway"}, {"name": "Alejandro Farm", "x": 9633.72, "y": -14916.7, "type": "Trading Post"}, {"name": "Costa Farm", "x": 9275.84, "y": -56067.4, "type": "Trading Post"}, {"name": "Moretti Plantation", "x": 73723.8, "y": -70902.8, "type": "Trading Post"}, {"name": "Castle Ruins", "x": 89949.0, "y": -100270.0, "type": "Dangerous"}, {"name": "Bloody Monastery", "x": -11394.5, "y": -172515.0, "type": "Dangerous"}, {"name": "Northern Heidel Quarry", "x": 36542.3, "y": -9346.36, "type": "Connection"}, {"name": "Serendia Shrine", "x": 22805.1, "y": -167375.0, "type": "Dangerous"}, {"name": "Lynch Ranch", "x": -42010.6, "y": -6764.91, "type": "Trading Post"}, {"name": "Northern Plain of Serendia", "x": -53939.5, "y": -43625.8, "type": "Connection"}, {"name": "Valencia Castle", "x": -24447.9, "y": -84013.4, "type": "Gateway"}, {"name": "Glish Swamp", "x": -34801.3, "y": -108074.0, "type": "Connection"}, {"name": "Southern Swamp", "x": 11557.2, "y": -134191.0, "type": "Connection"}, {"name": "Glish Ruins", "x": 37478.7, "y": -107868.0, "type": "Connection"}, {"name": "Northern Swamp", "x": 46353.8, "y": -70889.2, "type": "Connection"}, {"name": "Lynch Farm Ruins", "x": -17993.4, "y": -38133.4, "type": "Connection"}, {"name": "Biraghi Den", "x": -93594.6, "y": -27371.4, "type": "Dangerous"}, {"name": "Bradie Fortress", "x": -94348.0, "y": -70887.4, "type": "Dangerous"}, {"name": "Southern Neutral Zone", "x": -82512.8, "y": -142702.0, "type": "Dangerous"}, {"name": "Orc Camp", "x": -83330.8, "y": -98390.9, "type": "Connection"}, {"name": "Delphe Knights Castle", "x": -135824.0, "y": -51319.8, "type": "Gateway"}, {"name": "Watchtower", "x": -66903.9, "y": -106952.0, "type": "Connection"}, {"name": "Luciano Pietro Investment Bank", "x": 41632.6, "y": -49307.7, "type": ""}, {"name": "Siuta Investment Bank", "x": 39092.6, "y": -29184.7, "type": ""}, {"name": "Freharau Investment Bank", "x": -16699.5, "y": -120442.0, "type": ""}, {"name": "Larc Investment Bank", "x": -21722.6, "y": -120918.0, "type": ""}, {"name": "Specialties", "x": 7278.26, "y": -17968.5, "type": ""}, {"name": "Specialties", "x": -2974.05, "y": -129123.0, "type": ""}, {"name": "Specialties", "x": 37900.0, "y": -63445.2, "type": ""}, {"name": "Specialties", "x": 74262.8, "y": -72521.7, "type": ""}, {"name": "Pumpkin Farming", "x": 10181.4, "y": -14475.6, "type": ""}, {"name": "Honey Production", "x": 10606.4, "y": -14207.8, "type": ""}, {"name": "Wheat Farming", "x": 17481.0, "y": -55567.8, "type": ""}, {"name": "Pumpkin Farming", "x": 11484.6, "y": -55444.2, "type": ""}, {"name": "Specialties", "x": 10033.3, "y": -55486.8, "type": ""}, {"name": "Flax Farming", "x": 15094.6, "y": -56423.9, "type": ""}, {"name": "Wheat Farming", "x": 69729.6, "y": -67668.6, "type": ""}, {"name": "Flax Farming", "x": 71541.8, "y": -75542.9, "type": ""}, {"name": "Moretti Safe Zone", "x": 75120.1, "y": -70166.1, "type": "Connection"}, {"name": "Lumbering", "x": 88935.0, "y": -100208.0, "type": ""}, {"name": "Mining", "x": 34033.1, "y": 8039.14, "type": ""}, {"name": "Mining", "x": 33642.3, "y": 7985.83, "type": ""}, {"name": "Lumbering", "x": 35789.1, "y": -161535.0, "type": ""}, {"name": "Fleece Production", "x": -41945.0, "y": -6871.09, "type": ""}, {"name": "Gathering", "x": -53785.8, "y": -44130.8, "type": ""}, {"name": "Lumbering", "x": -54866.1, "y": -44270.6, "type": ""}, {"name": "NOT_A_NODE", "x": -18730.6, "y": -83345.1, "type": ""}, {"name": "NOT_A_NODE", "x": -19125.8, "y": -83574.6, "type": ""}, {"name": "NOT_A_NODE", "x": -18336.3, "y": -83586.3, "type": ""}, {"name": "NOT_A_NODE", "x": -18244.6, "y": -84101.7, "type": ""}, {"name": "Gathering", "x": -33091.4, "y": -108752.0, "type": ""}, {"name": "Mining", "x": -33068.0, "y": -109096.0, "type": ""}, {"name": "Specialties", "x": -16430.5, "y": -111168.0, "type": ""}, {"name": "Gathering", "x": 11632.4, "y": -133152.0, "type": ""}, {"name": "Mining", "x": 12009.4, "y": -132752.0, "type": ""}, {"name": "Gathering", "x": 37257.2, "y": -109693.0, "type": ""}, {"name": "Excavation", "x": 37442.1, "y": -108959.0, "type": ""}, {"name": "Gathering", "x": 45045.1, "y": -72349.7, "type": ""}, {"name": "Mining", "x": 43403.6, "y": -72912.0, "type": ""}, {"name": "Gathering", "x": -25336.6, "y": -35864.1, "type": ""}, {"name": "Excavation", "x": -24555.4, "y": -35690.0, "type": ""}, {"name": "Calpheon", "x": -249688.0, "y": -53111.5, "type": "City"}, {"name": "Keplan", "x": -152284.0, "y": -146923.0, "type": "Town"}, {"name": "Florin", "x": -166912.0, "y": 52937.7, "type": "Town"}, {"name": "Port Epheria", "x": -351453.0, "y": 41756.0, "type": "Town"}, {"name": "Enrique Encarotia Investment Bank", "x": -235997.0, "y": -80897.1, "type": ""}, {"name": "Lehard Mertenan Investment Bank", "x": -264318.0, "y": -65791.6, "type": ""}, {"name": "Luolo Grebe Investment Bank", "x": -262303.0, "y": -40374.9, "type": ""}, {"name": "Trent", "x": -378010.0, "y": -229988.0, "type": "Town"}, {"name": "Behr", "x": -286407.0, "y": -239541.0, "type": "Town"}, {"name": "Basquean Ljurik Investment Bank", "x": -242309.0, "y": -44340.5, "type": ""}, {"name": "Norma Leight Investment Bank", "x": -211720.0, "y": -7894.47, "type": ""}, {"name": "Valentine Investment Bank", "x": -167448.0, "y": 52189.7, "type": ""}, {"name": "Specialties", "x": -200686.0, "y": -64347.7, "type": ""}, {"name": "Specialties", "x": -348216.0, "y": -248779.0, "type": ""}, {"name": "Specialties", "x": -213837.0, "y": -30988.5, "type": ""}, {"name": "Specialties", "x": -291796.0, "y": -70967.2, "type": ""}, {"name": "Specialties", "x": -249487.0, "y": -137326.0, "type": ""}, {"name": "Specialties", "x": -118806.0, "y": -176983.0, "type": ""}, {"name": "Specialties", "x": -136745.0, "y": -53330.8, "type": ""}, {"name": "Abandoned Land", "x": -276109.0, "y": -28524.3, "type": "Connection"}, {"name": "Quint Hill", "x": -297268.0, "y": 31545.5, "type": "Connection"}, {"name": "Anti-Troll Fortification", "x": -260018.0, "y": 8127.2, "type": "Gateway"}, {"name": "Bree Tree Ruins", "x": -220965.0, "y": 52395.8, "type": "Dangerous"}, {"name": "Khuruto Cave", "x": -170427.0, "y": 2724.17, "type": "Dangerous"}, {"name": "Delphe Outpost", "x": -124345.0, "y": 5352.93, "type": "Gateway"}, {"name": "Karanda Ridge", "x": -125680.0, "y": 34039.1, "type": "Dangerous"}, {"name": "Northern Wheat Plantation", "x": -214474.0, "y": -5728.0, "type": "Town"}, {"name": "Old Dandelion", "x": -169784.0, "y": -9806.7, "type": "Dangerous"}, {"name": "Contaminated Farm", "x": -243516.0, "y": -23043.6, "type": "Trading Post"}, {"name": "Caphras Cave", "x": -184677.0, "y": 49966.9, "type": "Dangerous"}, {"name": "Isolated Sentry Post", "x": -287721.0, "y": -1425.13, "type": "Gateway"}, {"name": "Dias Farm", "x": -212294.0, "y": -33106.2, "type": "Trading Post"}, {"name": "Epheria Sentry Post", "x": -370114.0, "y": 28670.7, "type": "Gateway"}, {"name": "Epheria Valley", "x": -347953.0, "y": 2700.7, "type": "Connection"}, {"name": "Cohen Farm", "x": -315126.0, "y": -42696.3, "type": "Trading Post"}, {"name": "Elder's Bridge Post", "x": -261807.0, "y": 59893.5, "type": "Gateway"}, {"name": "Bernianto Farm", "x": -229908.0, "y": 11573.8, "type": "Trading Post"}, {"name": "Marni Cave Path", "x": -195164.0, "y": -110884.0, "type": "Gateway"}, {"name": "Oze Pass", "x": -133846.0, "y": -76098.7, "type": "Dangerous"}, {"name": "Marni Farm Ruins", "x": -165392.0, "y": -83109.9, "type": "Connection"}, {"name": "Keplan Hill", "x": -149597.0, "y": -167071.0, "type": "Connection"}, {"name": "Saunil Camp", "x": -212642.0, "y": -188637.0, "type": "Dangerous"}, {"name": "Primal Giant Post", "x": -125677.0, "y": -235919.0, "type": "Dangerous"}, {"name": "Saunil Battlefield", "x": -252379.0, "y": -188019.0, "type": "Dangerous"}, {"name": "Trina Fort", "x": -222470.0, "y": -157556.0, "type": "Gateway"}, {"name": "Trina Beacon Towers", "x": -227703.0, "y": -141850.0, "type": "Gateway"}, {"name": "Marni's Lab", "x": -187999.0, "y": -125444.0, "type": "Dangerous"}, {"name": "Falres Dirt Farm", "x": -199177.0, "y": -61438.5, "type": "Trading Post"}, {"name": "Hexe Stone Wall", "x": -173761.0, "y": -245348.0, "type": "Connection"}, {"name": "Bain Farmland", "x": -247841.0, "y": -137715.0, "type": "Trading Post"}, {"name": "North Abandoned Quarry", "x": -151263.0, "y": -122612.0, "type": "Dangerous"}, {"name": "Oberen Farm", "x": -230592.0, "y": -112313.0, "type": "Trading Post"}, {"name": "Beacon Entrance Post", "x": -214647.0, "y": -131541.0, "type": "Trading Post"}, {"name": "Abandoned Quarry", "x": -158265.0, "y": -184606.0, "type": "Trading Post"}, {"name": "Gehaku Plain", "x": -135294.0, "y": -206059.0, "type": "Connection"}, {"name": "Keplan Vicinity", "x": -121835.0, "y": -138057.0, "type": "Connection"}, {"name": "Gianin Farm", "x": -120011.0, "y": -177863.0, "type": "Trading Post"}, {"name": "Serendia Western Gateway", "x": -96472.8, "y": -176601.0, "type": "Gateway"}, {"name": "Glutoni Cave", "x": -175384.0, "y": -145388.0, "type": "Dangerous"}, {"name": "Quarry Byway", "x": -122979.0, "y": -111354.0, "type": "Trading Post"}, {"name": "Keplan Quarry", "x": -155252.0, "y": -132316.0, "type": "Connection"}, {"name": "Oze's House", "x": -148184.0, "y": -105679.0, "type": "Dangerous"}, {"name": "Dane Canyon", "x": -189717.0, "y": -202735.0, "type": "Trading Post"}, {"name": "Tarte Rock Fork", "x": -190545.0, "y": -162334.0, "type": "Connection"}, {"name": "Calpheon Castle Site", "x": -329706.0, "y": -76644.5, "type": "Connection"}, {"name": "Calpheon Castle", "x": -348603.0, "y": -53398.3, "type": "Gateway"}, {"name": "Treant Forest", "x": -407100.0, "y": -173996.0, "type": "Dangerous"}, {"name": "North Kaia Pier", "x": -318272.0, "y": -86543.3, "type": "Gateway"}, {"name": "Behr Riverhead", "x": -305267.0, "y": -186073.0, "type": "Connection"}, {"name": "Rhutum Outstation", "x": -342572.0, "y": -159821.0, "type": "Dangerous"}, {"name": "Rhutum Sentry Post", "x": -328998.0, "y": -132014.0, "type": "Trading Post"}, {"name": "Abandoned Monastery", "x": -368945.0, "y": -174832.0, "type": "Gateway"}, {"name": "Crioville", "x": -322795.0, "y": -253951.0, "type": "Trading Post"}, {"name": "Rhua Tree Stub", "x": -251690.0, "y": -208681.0, "type": "Connection"}, {"name": "South Kaia Pier", "x": -325066.0, "y": -104406.0, "type": "Gateway"}, {"name": "Gabino Farm", "x": -291135.0, "y": -68128.4, "type": "Trading Post"}, {"name": "Catfishman Camp", "x": -351826.0, "y": -95172.6, "type": "Dangerous"}, {"name": "Calpheon Castle Western Forest", "x": -384183.0, "y": -95148.4, "type": "Dangerous"}, {"name": "Mansha Forest", "x": -376017.0, "y": -123338.0, "type": "Trading Post"}, {"name": "Marie Cave", "x": -188309.0, "y": -248867.0, "type": "Connection"}, {"name": "Tobare's Cabin", "x": -381232.0, "y": -147982.0, "type": "Connection"}, {"name": "Lumberjack's Rest Area", "x": -390565.0, "y": -208654.0, "type": "Connection"}, {"name": "Longleaf Tree Sentry Post", "x": -347632.0, "y": -255743.0, "type": "Trading Post"}, {"name": "Longleaf Tree Forest", "x": -306195.0, "y": -262508.0, "type": "Dangerous"}, {"name": "Witch's Chapel", "x": -199813.0, "y": -276024.0, "type": "Dangerous"}, {"name": "Phoniel's Cabin", "x": -305538.0, "y": -146365.0, "type": "Connection"}, {"name": "Behr Downstream", "x": -273114.0, "y": -200792.0, "type": "Connection"}, {"name": "Hexe Sanctuary", "x": -231169.0, "y": -251607.0, "type": "Dangerous"}, {"name": "Phoniel's Cabin Entrance", "x": -275403.0, "y": -169823.0, "type": "Connection"}, {"name": "North Kaia Mountaintop", "x": -278722.0, "y": -105913.0, "type": "Dangerous"}, {"name": "Lake Kaia", "x": -341680.0, "y": -106931.0, "type": "Dangerous"}, {"name": "Calpheon Trade Exchange", "x": -246510.0, "y": -52626.4, "type": ""}, {"name": "Specialties", "x": -170904.0, "y": 55126.5, "type": ""}, {"name": "Florin_2", "x": -171013.0, "y": 54532.0, "type": ""}, {"name": "Florin_3", "x": -170625.0, "y": 55241.4, "type": ""}, {"name": "Marco Faust Investment Bank", "x": -151177.0, "y": -147547.0, "type": ""}, {"name": "Christine Cessory Investment Bank", "x": -159343.0, "y": -155806.0, "type": ""}, {"name": "Gathering", "x": -347437.0, "y": 3463.5, "type": ""}, {"name": "Specialties", "x": -362184.0, "y": 28908.8, "type": ""}, {"name": "Lumbering", "x": -300689.0, "y": 21119.1, "type": ""}, {"name": "Mining", "x": -300919.0, "y": 20562.9, "type": ""}, {"name": "Lumbering", "x": -221400.0, "y": 52330.6, "type": ""}, {"name": "Gathering", "x": -220867.0, "y": 52705.6, "type": ""}, {"name": "Excavation", "x": -221487.0, "y": 52714.3, "type": ""}, {"name": "Mining", "x": -172650.0, "y": 2504.37, "type": ""}, {"name": "Khuruto Cave", "x": -172514.0, "y": 1690.45, "type": "Dangerous"}, {"name": "NOT_A_NODE", "x": -123089.0, "y": 4453.42, "type": ""}, {"name": "NOT_A_NODE", "x": -123397.0, "y": 4010.95, "type": ""}, {"name": "NOT_A_NODE", "x": -122660.0, "y": 4044.19, "type": ""}, {"name": "Gathering", "x": -123305.0, "y": 33466.8, "type": ""}, {"name": "Karanda Ridge", "x": -122836.0, "y": 33487.4, "type": "Dangerous"}, {"name": "NOT_A_NODE", "x": -123276.0, "y": 33184.9, "type": ""}, {"name": "Wheat Farming", "x": -216898.0, "y": -15404.6, "type": ""}, {"name": "Barley Farming", "x": -220742.0, "y": 755.84, "type": ""}, {"name": "Paprika Farming", "x": -207216.0, "y": -8922.43, "type": ""}, {"name": "Lumbering", "x": -165424.0, "y": -18687.2, "type": ""}, {"name": "Old Dandelion", "x": -165935.0, "y": -16606.7, "type": "Dangerous"}, {"name": "Specialties", "x": -216883.0, "y": -8860.02, "type": ""}, {"name": "Northern Wheat Plantation Safe Zone", "x": -213847.0, "y": -7910.94, "type": ""}, {"name": "Lumbering", "x": -374671.0, "y": -124516.0, "type": ""}, {"name": "Excavation", "x": -381385.0, "y": -134281.0, "type": ""}, {"name": "Lumbering", "x": -305105.0, "y": -144985.0, "type": ""}, {"name": "Gathering", "x": -252993.0, "y": -212563.0, "type": ""}, {"name": "Lumbering", "x": -386595.0, "y": -202049.0, "type": ""}, {"name": "Gathering", "x": -344292.0, "y": -250009.0, "type": ""}, {"name": "Lumbering", "x": -309478.0, "y": -273222.0, "type": ""}, {"name": "Lumbering", "x": -404833.0, "y": -189831.0, "type": ""}, {"name": "Abandoned Monastery", "x": -363568.0, "y": -182896.0, "type": "Gateway"}, {"name": "Lumbering", "x": -196272.0, "y": -250386.0, "type": ""}, {"name": "Gathering", "x": -186584.0, "y": -253496.0, "type": ""}, {"name": "Excavation", "x": -252250.0, "y": -209316.0, "type": ""}, {"name": "Mining", "x": -333785.0, "y": -149402.0, "type": ""}, {"name": "Mining", "x": -302442.0, "y": -184848.0, "type": ""}, {"name": "Specialties", "x": -388294.0, "y": -230449.0, "type": ""}, {"name": "Mining", "x": -139248.0, "y": -193752.0, "type": ""}, {"name": "Mining", "x": -160121.0, "y": -124762.0, "type": ""}, {"name": "Lumbering", "x": -145573.0, "y": -76623.7, "type": ""}, {"name": "Oze Pass", "x": -133084.0, "y": -65716.7, "type": "Dangerous"}, {"name": "North Abandoned Quarry", "x": -144613.0, "y": -116678.0, "type": "Dangerous"}, {"name": "Abandoned Quarry", "x": -148118.0, "y": -193181.0, "type": "Trading Post"}, {"name": "Keplan Hill", "x": -153495.0, "y": -166590.0, "type": "Connection"}, {"name": "Gathering", "x": -184718.0, "y": -156402.0, "type": ""}, {"name": "Mining", "x": -177440.0, "y": -139851.0, "type": ""}, {"name": "Mining", "x": -173783.0, "y": -249124.0, "type": ""}, {"name": "Hexe Stone Wall", "x": -160098.0, "y": -241783.0, "type": "Connection"}, {"name": "Calpheon City Trade Zone", "x": -255070.0, "y": -31575.6, "type": ""}, {"name": "Calpheon Market Trade Zone", "x": -233823.0, "y": -48111.7, "type": ""}, {"name": "Calpheon Holy College Trade Zone", "x": -248857.0, "y": -76152.4, "type": ""}, {"name": "Specialties", "x": -286704.0, "y": -234095.0, "type": ""}, {"name": "Lema Island", "x": -56616.4, "y": 392036.0, "type": "Town"}, {"name": "Iliya Island", "x": 152406.0, "y": 297512.0, "type": "Town"}, {"name": "Pilava Island", "x": 249398.0, "y": 197916.0, "type": "Trading Post"}, {"name": "Delinghart Island", "x": 203901.0, "y": 201296.0, "type": "Connection"}, {"name": "Pujara Island", "x": 252417.0, "y": 298317.0, "type": "Connection"}, {"name": "Ajir Island", "x": 81332.4, "y": 330939.0, "type": "Connection"}, {"name": "Al-Naha Island", "x": 41573.8, "y": 367598.0, "type": "Connection"}, {"name": "Racid Island", "x": 73417.5, "y": 415322.0, "type": "Trading Post"}, {"name": "Baremi Island", "x": 15740.2, "y": 288923.0, "type": "Trading Post"}, {"name": "Weita Island", "x": 38211.1, "y": 256196.0, "type": "Connection"}, {"name": "Beiruwa Island", "x": 83757.4, "y": 169872.0, "type": "Trading Post"}, {"name": "Taramura Island", "x": 131585.0, "y": 196309.0, "type": "Connection"}, {"name": "Ostra Island", "x": 145306.0, "y": 215740.0, "type": "Connection"}, {"name": "Arakil Island", "x": 95475.3, "y": 221552.0, "type": "Connection"}, {"name": "Kanvera Island", "x": 80342.2, "y": 244027.0, "type": "Connection"}, {"name": "Orffs Island", "x": -61956.1, "y": 325836.0, "type": "Connection"}, {"name": "Tulu Island", "x": -90726.2, "y": 325649.0, "type": "Connection"}, {"name": "Luivano Island", "x": -43687.9, "y": 183465.0, "type": "Trading Post"}, {"name": "Duch Island", "x": -94535.6, "y": 213574.0, "type": "Trading Post"}, {"name": "Mariveno Island", "x": -26967.4, "y": 228829.0, "type": "Connection"}, {"name": "Paratama Island", "x": 39877.7, "y": 209785.0, "type": "Connection"}, {"name": "Eveto Island", "x": -80628.6, "y": 226097.0, "type": "Connection"}, {"name": "Balvege Island", "x": -97908.4, "y": 270167.0, "type": "Connection"}, {"name": "Marlene Island", "x": -67182.9, "y": 268006.0, "type": "Connection"}, {"name": "Invernen Island", "x": -119375.0, "y": 312046.0, "type": "Connection"}, {"name": "Angie Island", "x": -122017.0, "y": 241576.0, "type": "Connection"}, {"name": "Tashu Island", "x": -126823.0, "y": 429685.0, "type": "Connection"}, {"name": "Fish Drying Yard 1", "x": 262503.0, "y": 339004.0, "type": ""}, {"name": "Fish Drying Yard 2", "x": 260260.0, "y": 269058.0, "type": ""}, {"name": "Fish Drying Yard 1", "x": 255868.0, "y": 212160.0, "type": ""}, {"name": "Fish Drying Yard 2", "x": 275166.0, "y": 195249.0, "type": ""}, {"name": "Fish Drying Yard 1", "x": 172975.0, "y": 198866.0, "type": ""}, {"name": "Fish Drying Yard 2", "x": 206737.0, "y": 231599.0, "type": ""}, {"name": "Fish Drying Yard 1", "x": 156613.0, "y": 172796.0, "type": ""}, {"name": "Fish Drying Yard 2", "x": 118813.0, "y": 162317.0, "type": ""}, {"name": "Fish Drying Yard", "x": 70031.9, "y": 168649.0, "type": ""}, {"name": "Fish Drying Yard", "x": 66144.1, "y": 201408.0, "type": ""}, {"name": "Ephde Rune Island", "x": 6006.37, "y": 164907.0, "type": "Connection"}, {"name": "Fish Drying Yard", "x": -5889.18, "y": 193624.0, "type": ""}, {"name": "Fish Drying Yard 1", "x": -24974.6, "y": 267599.0, "type": ""}, {"name": "Fish Drying Yard", "x": -75361.4, "y": 165862.0, "type": ""}, {"name": "Fish Drying Yard 1", "x": -156579.0, "y": 214979.0, "type": ""}, {"name": "Fish Drying Yard 2", "x": -131495.0, "y": 207846.0, "type": ""}, {"name": "Fish Drying Yard 3", "x": -136137.0, "y": 272064.0, "type": ""}, {"name": "Fish Drying Yard 1", "x": -66876.1, "y": 309941.0, "type": ""}, {"name": "Fish Drying Yard 2", "x": -38636.0, "y": 313501.0, "type": ""}, {"name": "Fish Drying Yard 1", "x": -167666.0, "y": 309069.0, "type": ""}, {"name": "Fish Drying Yard", "x": -50214.5, "y": 376047.0, "type": ""}, {"name": "Fish Drying Yard 2", "x": -127052.0, "y": 357942.0, "type": ""}, {"name": "Marka Island", "x": -183679.0, "y": 243410.0, "type": "Connection"}, {"name": "Louruve Island", "x": -220383.0, "y": 234862.0, "type": "Trading Post"}, {"name": "Staren Island", "x": -251206.0, "y": 199565.0, "type": "Connection"}, {"name": "Lisz Island", "x": -222301.0, "y": 270105.0, "type": "Connection"}, {"name": "Narvo Island", "x": -184673.0, "y": 288729.0, "type": "Connection"}, {"name": "Albresser Island", "x": -322728.0, "y": 167626.0, "type": "Trading Post"}, {"name": "Eberdeen Island", "x": -364103.0, "y": 182612.0, "type": "Connection"}, {"name": "Oben Island", "x": -373508.0, "y": 227486.0, "type": "Connection"}, {"name": "Daton Island", "x": -431128.0, "y": 212277.0, "type": "Trading Post"}, {"name": "Dunde Island", "x": -377734.0, "y": 176892.0, "type": "Connection"}, {"name": "Barater Island", "x": -329238.0, "y": 145277.0, "type": "Connection"}, {"name": "Randis Island", "x": -388275.0, "y": 107027.0, "type": "Trading Post"}, {"name": "Serca Island", "x": -384785.0, "y": 92913.3, "type": "Connection"}, {"name": "Baeza Island", "x": -442054.0, "y": 80094.4, "type": "Connection"}, {"name": "Modric Island", "x": -462908.0, "y": 97955.1, "type": "Connection"}, {"name": "Theonil Island", "x": -491057.0, "y": 99239.1, "type": "Trading Post"}, {"name": "Teyamal Island", "x": -528128.0, "y": 78345.1, "type": "Connection"}, {"name": "Rameda Island", "x": -509122.0, "y": 158345.0, "type": "Connection"}, {"name": "Ginburrey Island", "x": -458410.0, "y": 151382.0, "type": "Connection"}, {"name": "Netnume Island", "x": -419407.0, "y": 197906.0, "type": "Connection"}, {"name": "Fish Drying Yard", "x": -396870.0, "y": 78284.9, "type": ""}, {"name": "Fish Drying Yard", "x": -449338.0, "y": 73026.1, "type": ""}, {"name": "Fish Drying Yard", "x": -475699.0, "y": 84585.0, "type": ""}, {"name": "Fish Drying Yard 1", "x": -526577.0, "y": 52494.7, "type": ""}, {"name": "Fish Drying Yard 2", "x": -509523.0, "y": 59222.9, "type": ""}, {"name": "Fish Drying Yard 1", "x": -399841.0, "y": 105982.0, "type": ""}, {"name": "Fish Drying Yard", "x": -468213.0, "y": 212065.0, "type": ""}, {"name": "Fish Drying Yard 2", "x": -391879.0, "y": 131364.0, "type": ""}, {"name": "Fish Drying Yard 1", "x": -396569.0, "y": 244060.0, "type": ""}, {"name": "Fish Drying Yard 2", "x": -358074.0, "y": 227851.0, "type": ""}, {"name": "Fish Drying Yard 1", "x": -343762.0, "y": 150812.0, "type": ""}, {"name": "Fish Drying Yard 2", "x": -309097.0, "y": 155690.0, "type": ""}, {"name": "Fish Drying Yard", "x": -329608.0, "y": 193108.0, "type": ""}, {"name": "Fish Drying Yard 1", "x": -251010.0, "y": 186827.0, "type": ""}, {"name": "Fish Drying Yard 2", "x": -270529.0, "y": 214114.0, "type": ""}, {"name": "Fish Drying Yard 1", "x": -247826.0, "y": 245640.0, "type": ""}, {"name": "Fish Drying Yard 2", "x": -263936.0, "y": 271953.0, "type": ""}, {"name": "Fish Drying Yard", "x": -208095.0, "y": 218413.0, "type": ""}, {"name": "Fish Drying Yard", "x": 44494.3, "y": 265664.0, "type": ""}, {"name": "Fish Drying Yard 2", "x": -15597.3, "y": 228148.0, "type": ""}, {"name": "Kuit Islands", "x": -348128.0, "y": 373379.0, "type": "Connection"}, {"name": "Almai Island", "x": -402676.0, "y": 335255.0, "type": "Connection"}, {"name": "Padix Island", "x": -350554.0, "y": 311588.0, "type": "Connection"}, {"name": "Teste Island", "x": -428798.0, "y": 317013.0, "type": "Connection"}, {"name": "Arita Island", "x": -227874.0, "y": 319177.0, "type": "Connection"}, {"name": "Shasha Island", "x": 255821.0, "y": 395946.0, "type": "Connection"}, {"name": "Rosevan Island", "x": 304928.0, "y": 408105.0, "type": "Connection"}, {"name": "Portanen Island", "x": 294017.0, "y": 458735.0, "type": "Connection"}, {"name": "Tinberra Island", "x": 228865.0, "y": 531195.0, "type": "Connection"}, {"name": "Lerao Island", "x": 273917.0, "y": 543923.0, "type": "Connection"}, {"name": "Altinova", "x": 367322.0, "y": -69079.4, "type": "City"}, {"name": "Mediah Northern Gateway", "x": 100712.0, "y": 115314.0, "type": "Gateway"}, {"name": "Sausan Garrison", "x": 224473.0, "y": 127019.0, "type": "Dangerous"}, {"name": "Stonetail Wasteland", "x": 209220.0, "y": 80233.9, "type": "Connection"}, {"name": "Mediah Northern Highlands", "x": 161901.0, "y": 121784.0, "type": "Connection"}, {"name": "Rumbling Land", "x": 119758.0, "y": -21369.2, "type": "Connection"}, {"name": "Kamasylve Temple", "x": 147349.0, "y": -32153.8, "type": "Connection"}, {"name": "Manes Hideout", "x": 189412.0, "y": -33886.2, "type": "Dangerous"}, {"name": "Asula Highlands", "x": 239854.0, "y": -48902.5, "type": "Connection"}, {"name": "Wandering Rogue Den", "x": 259590.0, "y": -101327.0, "type": "Dangerous"}, {"name": "Altinova Entrance", "x": 324113.0, "y": -72443.9, "type": "Connection"}, {"name": "Tarif", "x": 226814.0, "y": -73831.4, "type": "Town"}, {"name": "Alumn Rock Valley", "x": 303722.0, "y": -161376.0, "type": "Connection"}, {"name": "Abandoned Iron Mine", "x": 299741.0, "y": -135331.0, "type": "Dangerous"}, {"name": "Abun", "x": 384252.0, "y": -141575.0, "type": "Town"}, {"name": "Marni's 2nd Lab", "x": 388138.0, "y": -166789.0, "type": "Dangerous"}, {"name": "Stonebeak Shore", "x": 305927.0, "y": -37179.1, "type": "Connection"}, {"name": "Soldiers' Cemetery", "x": 179504.0, "y": -78294.3, "type": "Dangerous"}, {"name": "Omar Lava Cave", "x": 248680.0, "y": -13665.1, "type": "Trading Post"}, {"name": "The Mausoleum", "x": 128193.0, "y": 123316.0, "type": "Connection"}, {"name": "Sarma Outpost", "x": 246235.0, "y": 85100.2, "type": "Gateway"}, {"name": "Mediah Castle", "x": 308326.0, "y": 59308.4, "type": "Gateway"}, {"name": "Kusha", "x": 229641.0, "y": 68121.2, "type": "Town"}, {"name": "Canyon of Corruption", "x": 190819.0, "y": 36547.5, "type": "Dangerous"}, {"name": "Elric Shrine", "x": 178308.0, "y": 67981.7, "type": "Dangerous"}, {"name": "Ancient Ruins Excavation Site", "x": 170846.0, "y": 3233.09, "type": "Connection"}, {"name": "Helms Post", "x": 123457.0, "y": 58059.8, "type": "Dangerous"}, {"name": "Stonetail Horse Ranch", "x": 221268.0, "y": -1184.55, "type": "Trading Post"}, {"name": "Ahto Farm", "x": 205023.0, "y": -19649.3, "type": "Connection"}, {"name": "Shuri Farm", "x": 196019.0, "y": 24346.1, "type": "Trading Post"}, {"name": "Kasula Farm", "x": 244468.0, "y": -77103.9, "type": "Trading Post"}, {"name": "Ancient Fissure", "x": 130821.0, "y": 25608.3, "type": "Connection"}, {"name": "Mediah Shore", "x": 253241.0, "y": 37112.4, "type": "Connection"}, {"name": "Highland Junction", "x": 270055.0, "y": -78059.9, "type": "Connection"}, {"name": "Splashing Point", "x": 310603.0, "y": -172900.0, "type": "Trading Post"}, {"name": "Sausan Garrison Wharf", "x": 246298.0, "y": 140074.0, "type": "Connection"}, {"name": "Abandoned Iron Mine Rhutum District", "x": 358051.0, "y": -128020.0, "type": "Dangerous"}, {"name": "Abandoned Iron Mine Saunil District", "x": 347325.0, "y": -163288.0, "type": "Dangerous"}, {"name": "Abandoned Iron Mine Entrance", "x": 285389.0, "y": -111385.0, "type": "Connection"}, {"name": "Awakening Bell", "x": 293349.0, "y": -24943.3, "type": "Connection"}, {"name": "Tungrad Forest", "x": 165486.0, "y": -89749.2, "type": "Dangerous"}, {"name": "Hasrah Cliff", "x": 196305.0, "y": -156100.0, "type": "Connection"}, {"name": "Zigmund Investment Bank", "x": 370701.0, "y": -48956.8, "type": ""}, {"name": "Gulabi Investment Bank", "x": 343397.0, "y": -30315.7, "type": ""}, {"name": "Quina Investment Bank", "x": 365169.0, "y": -72295.2, "type": ""}, {"name": "Neruda Shen Investment Bank", "x": 362724.0, "y": -62025.1, "type": ""}, {"name": "Specialties", "x": 225249.0, "y": -80765.3, "type": ""}, {"name": "Specialties", "x": 245810.0, "y": -79498.8, "type": ""}, {"name": "Specialties", "x": 206973.0, "y": -17378.3, "type": ""}, {"name": "Specialties", "x": 196331.0, "y": 18377.4, "type": ""}, {"name": "Specialties", "x": 224887.0, "y": 76363.6, "type": ""}, {"name": "Specialties", "x": 221201.0, "y": 4795.3, "type": ""}, {"name": "Specialties", "x": 382420.0, "y": -142844.0, "type": ""}, {"name": "Specialties", "x": 309753.0, "y": -173716.0, "type": ""}, {"name": "Cotton Farming", "x": 208412.0, "y": -18042.8, "type": ""}, {"name": "Aloe Farming", "x": 201539.0, "y": -15057.7, "type": ""}, {"name": "Mining", "x": 256170.0, "y": -17905.8, "type": ""}, {"name": "Mining", "x": 256275.0, "y": -13662.9, "type": ""}, {"name": "Sweet Potato Farming", "x": 197362.0, "y": 18772.4, "type": ""}, {"name": "Cotton Farming", "x": 245325.0, "y": -74068.9, "type": ""}, {"name": "Cinnamon Farming", "x": 245070.0, "y": -80181.6, "type": ""}, {"name": "Mining", "x": 347468.0, "y": -136729.0, "type": ""}, {"name": "Mining", "x": 346438.0, "y": -134875.0, "type": ""}, {"name": "Gathering", "x": 181588.0, "y": 75581.1, "type": ""}, {"name": "Gathering", "x": 148082.0, "y": -30237.3, "type": ""}, {"name": "Lumbering", "x": 153896.0, "y": 121666.0, "type": ""}, {"name": "Mining", "x": 133209.0, "y": 25792.4, "type": ""}, {"name": "Gathering", "x": 385687.0, "y": -159425.0, "type": ""}, {"name": "Lumbering", "x": 185529.0, "y": 74950.7, "type": ""}, {"name": "Lumbering", "x": 153903.0, "y": 121000.0, "type": ""}, {"name": "Gathering", "x": 129779.0, "y": 25383.2, "type": ""}, {"name": "Mining", "x": 255497.0, "y": 36372.3, "type": ""}, {"name": "Lumbering", "x": 207525.0, "y": 81047.1, "type": ""}, {"name": "Excavation", "x": 168795.0, "y": 6925.89, "type": ""}, {"name": "Flax Farming", "x": 149062.0, "y": -32744.3, "type": ""}, {"name": "Valencia City", "x": 1026110.0, "y": 199132.0, "type": "City"}, {"name": "Altinova Gateway", "x": 382721.0, "y": -8650.74, "type": "Connection"}, {"name": "Rock Post", "x": 424615.0, "y": 10328.2, "type": "Gateway"}, {"name": "Gorgo Rock Belt", "x": 393805.0, "y": 39058.8, "type": "Connection"}, {"name": "Veteran's Canyon", "x": 422278.0, "y": 47373.0, "type": "Connection"}, {"name": "Cadry Ruins", "x": 436068.0, "y": 82597.9, "type": "Dangerous"}, {"name": "Kunid's Vacation Spot", "x": 403646.0, "y": 97768.1, "type": "Connection"}, {"name": "Leical Falls", "x": 382129.0, "y": 111543.0, "type": "Connection"}, {"name": "Pujiya Canyon", "x": 467184.0, "y": -15486.9, "type": "Connection"}, {"name": "Bashim Base", "x": 478608.0, "y": -83247.7, "type": "Dangerous"}, {"name": "Waragon Nest", "x": 573278.0, "y": -84465.1, "type": "Dangerous"}, {"name": "Deserted City of Runn", "x": 472077.0, "y": 165750.0, "type": "Trading Post"}, {"name": "Runn Gateway Intersection", "x": 485261.0, "y": 197883.0, "type": "Gateway"}, {"name": "Shakatu", "x": 579545.0, "y": 274679.0, "type": "Town"}, {"name": "Taphtar Plain", "x": 486449.0, "y": 28271.5, "type": "Dangerous"}, {"name": "Basilisk Den", "x": 380222.0, "y": 30348.3, "type": "Dangerous"}, {"name": "Barhan Gateway", "x": 522973.0, "y": 30237.3, "type": "Gateway"}, {"name": "Capotia", "x": 551976.0, "y": 38607.3, "type": "Connection"}, {"name": "Sand Grain Bazaar", "x": 590881.0, "y": 48856.9, "type": "Town"}, {"name": "NOT_A_NODE", "x": 570348.0, "y": 258741.0, "type": ""}, {"name": "Yalt Canyon", "x": 619716.0, "y": 307049.0, "type": "Connection"}, {"name": "Desert Naga Temple", "x": 544414.0, "y": 105746.0, "type": "Dangerous"}, {"name": "NOT_A_NODE", "x": 536376.0, "y": 185583.0, "type": ""}, {"name": "Pila Fe", "x": 461690.0, "y": 74836.4, "type": "Connection"}, {"name": "Pilgrim's Haven", "x": 679352.0, "y": 104564.0, "type": "Connection"}, {"name": "Hope Pier", "x": 541368.0, "y": 298828.0, "type": "Connection"}, {"name": "Gahaz Bandits' Lair", "x": 646653.0, "y": 335010.0, "type": "Dangerous"}, {"name": "Bambu Valley", "x": 715113.0, "y": 369438.0, "type": "Connection"}, {"name": "Iris Canyon", "x": 763431.0, "y": 406686.0, "type": "Connection"}, {"name": "Kmach Canyon", "x": 769587.0, "y": 391936.0, "type": "Connection"}, {"name": "Ibellab Oasis", "x": 734898.0, "y": 196590.0, "type": "Trading Post"}, {"name": "Pilgrim's Sanctum: Obedience", "x": 814773.0, "y": 278026.0, "type": "Connection"}, {"name": "Pilgrim's Sanctum: Abstinence", "x": 743190.0, "y": 144611.0, "type": "Connection"}, {"name": "Pilgrim's Sanctum: Sharing", "x": 891785.0, "y": 61744.8, "type": "Connection"}, {"name": "Pilgrim's Sanctum: Sincerity", "x": 821479.0, "y": -19086.4, "type": "Connection"}, {"name": "Pilgrim's Sanctum: Humility", "x": 892725.0, "y": -61532.3, "type": "Connection"}, {"name": "Pilgrim's Sanctum: Purity", "x": 971153.0, "y": 325.74, "type": "Connection"}, {"name": "Pilgrim's Sanctum: Fast", "x": 947765.0, "y": 160635.0, "type": "Connection"}, {"name": "Rakshan Observatory", "x": 948121.0, "y": 253076.0, "type": "Connection"}, {"name": "Scarlet Sand Chamber", "x": 524582.0, "y": 144240.0, "type": "Dangerous"}, {"name": "Valencia Castle", "x": 1150470.0, "y": 293950.0, "type": "Gateway"}, {"name": "Ancado Inner Harbor", "x": 976980.0, "y": 340744.0, "type": "Town"}, {"name": "Aakman", "x": 813638.0, "y": -110341.0, "type": "Trading Post"}, {"name": "Crescent Shrine", "x": 730906.0, "y": -201213.0, "type": "Dangerous"}, {"name": "Crescent Mountains", "x": 762533.0, "y": -162562.0, "type": "Connection"}, {"name": "Titium Valley", "x": 983393.0, "y": -164540.0, "type": "Dangerous"}, {"name": "NOT_A_NODE", "x": 632460.0, "y": 71023.7, "type": ""}, {"name": "NOT_A_NODE", "x": 773542.0, "y": 88379.6, "type": ""}, {"name": "Valencia Western Highlands", "x": 603168.0, "y": -12740.3, "type": "Connection"}, {"name": "Bazaar Farmland", "x": 577886.0, "y": 26613.0, "type": "Connection"}, {"name": "Shakatu Farmland", "x": 567974.0, "y": 285316.0, "type": "Connection"}, {"name": "Altas Farmland", "x": 997846.0, "y": 333242.0, "type": "Connection"}, {"name": "Erdal Farm", "x": 995260.0, "y": 224866.0, "type": "Connection"}, {"name": "Valencia Plantation", "x": 990646.0, "y": 197386.0, "type": "Connection"}, {"name": "Fohalam Farm", "x": 985941.0, "y": 176471.0, "type": "Connection"}, {"name": "Sokota Island", "x": 338207.0, "y": 139124.0, "type": "Connection"}, {"name": "Riyed Island", "x": 384653.0, "y": 180159.0, "type": "Connection"}, {"name": "Esfah Island", "x": 418037.0, "y": 243233.0, "type": "Connection"}, {"name": "Tigris Island", "x": 412178.0, "y": 257633.0, "type": "Connection"}, {"name": "Shirna Island", "x": 448990.0, "y": 262620.0, "type": "Connection"}, {"name": "Halmad Island", "x": 564386.0, "y": 331846.0, "type": "Connection"}, {"name": "Kashuma Island", "x": 574681.0, "y": 366749.0, "type": "Connection"}, {"name": "Orisha Island", "x": 423766.0, "y": 313649.0, "type": "Trading Post"}, {"name": "Boa Island", "x": 420347.0, "y": 367649.0, "type": "Connection"}, {"name": "Kisleev Crag", "x": 373040.0, "y": 103310.0, "type": "Connection"}, {"name": "Valencia Castle Site", "x": 1109330.0, "y": 229319.0, "type": "Connection"}, {"name": "Ancado Coast", "x": 895018.0, "y": 349452.0, "type": "Connection"}, {"name": "Derko Island", "x": 843205.0, "y": 415735.0, "type": "Connection"}, {"name": "Shakatu Abandoned Pier", "x": 504922.0, "y": 264149.0, "type": "Connection"}, {"name": "Areha Palm Forest", "x": 1232340.0, "y": 213470.0, "type": "Connection"}, {"name": "Arehaza", "x": 1267170.0, "y": 177948.0, "type": "City"}, {"name": "Muiquun", "x": 1092940.0, "y": -135983.0, "type": "Town"}, {"name": "Central Cantusa", "x": 1244310.0, "y": 77137.1, "type": "Connection"}, {"name": "Cantusa Desert", "x": 1163710.0, "y": -154498.0, "type": "Connection"}, {"name": "Pila Ku Jail", "x": 1144130.0, "y": -80992.9, "type": "Dangerous"}, {"name": "Northern Sand Dune", "x": 1259350.0, "y": 283280.0, "type": "Connection"}, {"name": "Gavinya Volcano Zone", "x": 1146720.0, "y": 454797.0, "type": "Connection"}, {"name": "Gavinya Great Crater", "x": 1093380.0, "y": 488275.0, "type": "Connection"}, {"name": "Gavinya Coastal Cliff", "x": 1163610.0, "y": 514146.0, "type": "Connection"}, {"name": "Roud Sulfur Works", "x": 1084880.0, "y": 418194.0, "type": "Dangerous"}, {"name": "Ivory Wasteland", "x": 988609.0, "y": 487834.0, "type": "Connection"}, {"name": "Ivero Cliff", "x": 928558.0, "y": 468371.0, "type": "Connection"}, {"name": "Dona Rocky Mountain", "x": 1173590.0, "y": 11589.3, "type": "Connection"}, {"name": "Hakoven Island", "x": 1241700.0, "y": 560845.0, "type": "Connection"}, {"name": "Ross Sea (1400)", "x": -652686.0, "y": 12522.8, "type": "Connection"}, {"name": "Ross Sea (1401)", "x": -627849.0, "y": 255638.0, "type": "Connection"}, {"name": "Ross Sea (1402)", "x": -576597.0, "y": 447833.0, "type": "Connection"}, {"name": "Ross Sea (1403)", "x": -410018.0, "y": 562615.0, "type": "Connection"}, {"name": "Ross Sea (1404)", "x": -192808.0, "y": 601192.0, "type": "Connection"}, {"name": "Ross Sea (1405)", "x": 63082.1, "y": 677838.0, "type": "Connection"}, {"name": "Ross Sea (1406)", "x": 332283.0, "y": 652615.0, "type": "Connection"}, {"name": "Margoria (1407)", "x": 295404.0, "y": 908709.0, "type": "Connection"}, {"name": "Margoria (1408)", "x": -12662.5, "y": 844692.0, "type": "Connection"}, {"name": "Margoria (1409)", "x": -295147.0, "y": 793638.0, "type": "Connection"}, {"name": "Margoria (1410)", "x": -525285.0, "y": 716006.0, "type": "Connection"}, {"name": "Margoria (1411)", "x": -720275.0, "y": 572522.0, "type": "Connection"}, {"name": "Margoria (1412)", "x": -819997.0, "y": 307282.0, "type": "Connection"}, {"name": "Margoria (1413)", "x": -883878.0, "y": 76558.3, "type": "Connection"}, {"name": "Margoria (1414)", "x": -1127050.0, "y": 229996.0, "type": "Connection"}, {"name": "Margoria (1415)", "x": -997896.0, "y": 460630.0, "type": "Connection"}, {"name": "Margoria (1416)", "x": -845204.0, "y": 691411.0, "type": "Connection"}, {"name": "Margoria (1417)", "x": -629032.0, "y": 858190.0, "type": "Connection"}, {"name": "Margoria (1418)", "x": -395624.0, "y": 947335.0, "type": "Connection"}, {"name": "Margoria (1419)", "x": -115741.0, "y": 1048790.0, "type": "Connection"}, {"name": "Margoria (1420)", "x": 165828.0, "y": 1190830.0, "type": "Connection"}, {"name": "Margoria (1421)", "x": -175.99, "y": 1394370.0, "type": "Connection"}, {"name": "Margoria (1422)", "x": -255960.0, "y": 1241030.0, "type": "Connection"}, {"name": "Margoria (1423)", "x": -532112.0, "y": 1101390.0, "type": "Connection"}, {"name": "Margoria (1424)", "x": -760568.0, "y": 968993.0, "type": "Connection"}, {"name": "Margoria (1425)", "x": -959850.0, "y": 831335.0, "type": "Connection"}, {"name": "Margoria (1426)", "x": -1127470.0, "y": 627277.0, "type": "Connection"}, {"name": "Margoria (1427)", "x": -1293620.0, "y": 435047.0, "type": "Connection"}, {"name": "Margoria (1428)", "x": -1407920.0, "y": 651404.0, "type": "Connection"}, {"name": "Margoria (1429)", "x": -1249760.0, "y": 798736.0, "type": "Connection"}, {"name": "Margoria (1430)", "x": -1025060.0, "y": 897476.0, "type": "Connection"}, {"name": "Margoria (1431)", "x": -866210.0, "y": 1044800.0, "type": "Connection"}, {"name": "Margoria (1432)", "x": -678605.0, "y": 1228070.0, "type": "Connection"}, {"name": "Margoria (1433)", "x": -448463.0, "y": 1394330.0, "type": "Connection"}, {"name": "Margoria (1434)", "x": -256179.0, "y": 1534820.0, "type": "Connection"}, {"name": "Juur Sea (1435)", "x": -485456.0, "y": 1625770.0, "type": "Connection"}, {"name": "Juur Sea (1436)", "x": -628104.0, "y": 1510620.0, "type": "Connection"}, {"name": "Juur Sea (1437)", "x": -797279.0, "y": 1317240.0, "type": "Connection"}, {"name": "Juur Sea (1438)", "x": -944380.0, "y": 1117870.0, "type": "Connection"}, {"name": "Juur Sea (1439)", "x": -1172390.0, "y": 967917.0, "type": "Connection"}, {"name": "Juur Sea (1440)", "x": -1273330.0, "y": 933392.0, "type": "Connection"}, {"name": "Juur Sea (1441)", "x": -1474840.0, "y": 871418.0, "type": "Connection"}, {"name": "Vadabin (1442)", "x": -1524540.0, "y": 1020710.0, "type": "Connection"}, {"name": "Vadabin (1443)", "x": -1458810.0, "y": 1013400.0, "type": "Connection"}, {"name": "Vadabin (1444)", "x": -914792.0, "y": 1429670.0, "type": "Connection"}, {"name": "Vadabin (1445)", "x": -917473.0, "y": 1521750.0, "type": "Connection"}, {"name": "Vadabin (1446)", "x": -758248.0, "y": 1564030.0, "type": "Connection"}, {"name": "Vadabin (1447)", "x": -673038.0, "y": 1651290.0, "type": "Connection"}, {"name": "Vadabin (1448)", "x": -582609.0, "y": 1696590.0, "type": "Connection"}, {"name": "Vadabin (1449)", "x": -1056870.0, "y": 1506020.0, "type": "Connection"}, {"name": "Margoria (Vell's Realm)", "x": -89590.8, "y": 946651.0, "type": "Connection"}, {"name": "Gathering", "x": 391618.0, "y": 43352.2, "type": ""}, {"name": "Lumbering", "x": 388909.0, "y": 59607.3, "type": ""}, {"name": "Mining", "x": 377857.0, "y": 29520.4, "type": ""}, {"name": "Lumbering", "x": 422121.0, "y": 53201.7, "type": ""}, {"name": "Gathering", "x": 402775.0, "y": 98005.3, "type": ""}, {"name": "Gathering", "x": 382450.0, "y": 112101.0, "type": ""}, {"name": "Mining", "x": 466787.0, "y": -15901.2, "type": ""}, {"name": "Mining", "x": 476003.0, "y": -79763.4, "type": ""}, {"name": "Gathering", "x": 551291.0, "y": 39013.1, "type": ""}, {"name": "Mining", "x": 555551.0, "y": 39741.1, "type": ""}, {"name": "NOT_A_NODE", "x": 535007.0, "y": 185547.0, "type": ""}, {"name": "Lumbering", "x": 682860.0, "y": 109292.0, "type": ""}, {"name": "Mining", "x": 674969.0, "y": 109279.0, "type": ""}, {"name": "Gathering", "x": 713106.0, "y": 368971.0, "type": ""}, {"name": "Gathering", "x": 762895.0, "y": 404945.0, "type": ""}, {"name": "Lumbering", "x": 763276.0, "y": 407455.0, "type": ""}, {"name": "Mining", "x": 769160.0, "y": 391130.0, "type": ""}, {"name": "Gathering", "x": 769078.0, "y": 391721.0, "type": ""}, {"name": "Gathering", "x": 831126.0, "y": -114888.0, "type": ""}, {"name": "Gathering", "x": 723373.0, "y": -199362.0, "type": ""}, {"name": "Mining", "x": 734679.0, "y": -203390.0, "type": ""}, {"name": "Mining", "x": 763119.0, "y": -161980.0, "type": ""}, {"name": "Mining", "x": 761743.0, "y": -163236.0, "type": ""}, {"name": "Gathering", "x": 987091.0, "y": -161613.0, "type": ""}, {"name": "Gathering", "x": 979806.0, "y": -171684.0, "type": ""}, {"name": "Lumbering", "x": 982755.0, "y": -157160.0, "type": ""}, {"name": "Nutmeg", "x": 573573.0, "y": 28577.5, "type": ""}, {"name": "Teff", "x": 573824.0, "y": 26605.3, "type": ""}, {"name": "Fig", "x": 571046.0, "y": 286301.0, "type": ""}, {"name": "Fig", "x": 565279.0, "y": 280271.0, "type": ""}, {"name": "Star Anise", "x": 565997.0, "y": 278040.0, "type": ""}, {"name": "Teff", "x": 1000270.0, "y": 334191.0, "type": ""}, {"name": "Teff", "x": 1000490.0, "y": 330263.0, "type": ""}, {"name": "Pistachio", "x": 992162.0, "y": 224206.0, "type": ""}, {"name": "Date Palm", "x": 994845.0, "y": 217973.0, "type": ""}, {"name": "Pistachio", "x": 989330.0, "y": 196851.0, "type": ""}, {"name": "Date Palm", "x": 991134.0, "y": 197034.0, "type": ""}, {"name": "Freekeh", "x": 991117.0, "y": 192661.0, "type": ""}, {"name": "Teff", "x": 993123.0, "y": 181912.0, "type": ""}, {"name": "Teff", "x": 985749.0, "y": 180897.0, "type": ""}, {"name": "Specialties", "x": 585112.0, "y": 22439.1, "type": ""}, {"name": "Specialties", "x": 568152.0, "y": 278340.0, "type": ""}, {"name": "Specialties", "x": 1004750.0, "y": 336542.0, "type": ""}, {"name": "Specialties", "x": 995880.0, "y": 224290.0, "type": ""}, {"name": "Specialties", "x": 991052.0, "y": 198385.0, "type": ""}, {"name": "Specialties", "x": 988365.0, "y": 172896.0, "type": ""}, {"name": "Atui Balacs Investment Bank", "x": 594170.0, "y": 49872.0, "type": ""}, {"name": "Godul Lateman Investment Bank", "x": 582870.0, "y": 38485.1, "type": ""}, {"name": "Taphtar Investment Bank", "x": 580279.0, "y": 282088.0, "type": ""}, {"name": "Valgon Investment Bank", "x": 582923.0, "y": 285743.0, "type": ""}, {"name": "Yis Kunjamin Investment Bank", "x": 1040870.0, "y": 222435.0, "type": ""}, {"name": "Zahad Investment Bank", "x": 1033380.0, "y": 198240.0, "type": ""}, {"name": "Excavation", "x": 888472.0, "y": -62350.0, "type": ""}, {"name": "Lumbering", "x": 1224640.0, "y": 211907.0, "type": ""}, {"name": "Lumbering", "x": 1236760.0, "y": 204997.0, "type": ""}, {"name": "Mining", "x": 1162520.0, "y": 517492.0, "type": ""}, {"name": "Gathering", "x": 1171440.0, "y": 508922.0, "type": ""}, {"name": "Mining", "x": 1091340.0, "y": 490771.0, "type": ""}, {"name": "Gathering", "x": 926953.0, "y": 469324.0, "type": ""}, {"name": "Gathering", "x": 1258540.0, "y": 282906.0, "type": ""}, {"name": "Mining", "x": 1148030.0, "y": 466038.0, "type": ""}, {"name": "Mining", "x": 1156640.0, "y": 460848.0, "type": ""}, {"name": "Specialties", "x": 1269750.0, "y": 175632.0, "type": ""}, {"name": "Specialties", "x": 1096060.0, "y": -133410.0, "type": ""}, {"name": "Silk Culture", "x": 997044.0, "y": 222054.0, "type": ""}, {"name": "Excavation", "x": 1087770.0, "y": 410879.0, "type": ""}, {"name": "Kamasylvia Vicinity", "x": -251721.0, "y": -306699.0, "type": "Dangerous"}, {"name": "Lemoria Guard Post", "x": -261640.0, "y": -339667.0, "type": "Gateway"}, {"name": "Atanis Pond", "x": -282919.0, "y": -362501.0, "type": "Connection"}, {"name": "Caduil Forest", "x": -307401.0, "y": -378909.0, "type": "Connection"}, {"name": "Old Wisdom Tree", "x": -363066.0, "y": -443196.0, "type": "Town"}, {"name": "Shady Tree Forest", "x": -387619.0, "y": -471699.0, "type": "Connection"}, {"name": "Navarn Steppe", "x": -412310.0, "y": -434504.0, "type": "Dangerous"}, {"name": "Central Lemoria Camp", "x": -438678.0, "y": -378107.0, "type": "Gateway"}, {"name": "Manshaum Forest", "x": -376867.0, "y": -359396.0, "type": "Dangerous"}, {"name": "Holo Forest", "x": -448067.0, "y": -352759.0, "type": "Connection"}, {"name": "Viv Foretta Hamlet", "x": -315901.0, "y": -310967.0, "type": "Trading Post"}, {"name": "Valtarra Mountains", "x": -344205.0, "y": -304449.0, "type": "Connection"}, {"name": "Valtarra - Altar of Training", "x": -379536.0, "y": -302465.0, "type": "Dangerous"}, {"name": "Mirumok Ruins", "x": -442851.0, "y": -318815.0, "type": "Dangerous"}, {"name": "Lemoria Beacon Towers", "x": -448692.0, "y": -473357.0, "type": "Gateway"}, {"name": "Southeast Kamasylvia", "x": -552073.0, "y": -510788.0, "type": "Connection"}, {"name": "Western Valtarra Mountains", "x": -458750.0, "y": -244894.0, "type": "Connection"}, {"name": "Acher Guard Post", "x": -513260.0, "y": -222685.0, "type": "Gateway"}, {"name": "Loopy Tree Forest", "x": -539099.0, "y": -211090.0, "type": "Dangerous"}, {"name": "Tooth Fairy Forest", "x": -586038.0, "y": -315386.0, "type": "Dangerous"}, {"name": "Tooth Fairy Cabin", "x": -540580.0, "y": -336204.0, "type": "Town"}, {"name": "White Wood Forest", "x": -499265.0, "y": -364348.0, "type": "Connection"}, {"name": "Lake Flondor", "x": -501768.0, "y": -409491.0, "type": "Trading Post"}, {"name": "Grána", "x": -513575.0, "y": -458652.0, "type": "City"}, {"name": "Polly's Forest", "x": -558679.0, "y": -421027.0, "type": "Connection"}, {"name": "Southern Kamasylvia", "x": -493490.0, "y": -498036.0, "type": "Connection"}, {"name": "Gyfin Rhasia Temple", "x": -527622.0, "y": -513314.0, "type": "Dangerous"}, {"name": "Krogdalo's Trace", "x": -585674.0, "y": -407890.0, "type": "Connection"}, {"name": "Looney Cabin", "x": -576184.0, "y": -438959.0, "type": "Connection"}, {"name": "Weenie Cabin", "x": -584660.0, "y": -375443.0, "type": "Connection"}, {"name": "Ash Forest", "x": -507197.0, "y": -162523.0, "type": "Dangerous"}, {"name": "Yianaros's Field", "x": -470357.0, "y": -259848.0, "type": "Connection"}, {"name": "Okiara River", "x": -478474.0, "y": -478203.0, "type": "Connection"}, {"name": "Farming", "x": -314149.0, "y": -310542.0, "type": ""}, {"name": "Specialty(?)", "x": -283077.0, "y": -362875.0, "type": "Specialty"}, {"name": "Lumbering", "x": -387039.0, "y": -470991.0, "type": ""}, {"name": "Mining", "x": -444599.0, "y": -347418.0, "type": ""}, {"name": "Excavation", "x": -582461.0, "y": -309341.0, "type": ""}, {"name": "Lumbering", "x": -541823.0, "y": -214535.0, "type": ""}, {"name": "Gathering", "x": -561654.0, "y": -419091.0, "type": ""}, {"name": "Grow Mushroom", "x": -588298.0, "y": -376774.0, "type": ""}, {"name": "Grow Mushroom", "x": -577230.0, "y": -433172.0, "type": ""}, {"name": "Mining", "x": -501370.0, "y": -410700.0, "type": ""}, {"name": "Investment Bank", "x": -492405.0, "y": -466199.0, "type": ""}, {"name": "Investment Bank", "x": -503910.0, "y": -431959.0, "type": ""}, {"name": "Mining", "x": -490156.0, "y": -500471.0, "type": ""}, {"name": "Acher Western Camp", "x": -604782.0, "y": -289683.0, "type": "Gateway"}, {"name": "Acher Southern Camp", "x": -506567.0, "y": -491998.0, "type": "Gateway"}, {"name": "Roud Sulfur Mine", "x": 1105760.0, "y": 440291.0, "type": "Connection"}, {"name": "Duvencrune", "x": -48357.4, "y": -404589.0, "type": "City"}, {"name": "Duvencrune Farmland", "x": -73674.8, "y": -418209.0, "type": "Connection"}, {"name": "UnKnown", "x": -16600.6, "y": -427630.0, "type": "City"}, {"name": "Ahib Conflict Zone", "x": -244569.0, "y": -402057.0, "type": "Gateway"}, {"name": "Akum Rocky Mountain", "x": -234036.0, "y": -382428.0, "type": "Dangerous"}, {"name": "Khalk Canyon", "x": -212470.0, "y": -424151.0, "type": "Connection"}, {"name": "Sherekhan Necropolis", "x": -156851.0, "y": -365116.0, "type": "Connection"}, {"name": "Garmoth's Nest", "x": -21964.1, "y": -329086.0, "type": "Dangerous"}, {"name": "Harak's Shelter", "x": -109449.0, "y": -387071.0, "type": "Connection"}, {"name": "Morning Fog Post", "x": -83794.0, "y": -364436.0, "type": "Connection"}, {"name": "Night Crow Post", "x": 20434.7, "y": -333830.0, "type": "Gateway"}, {"name": "Windy Peak", "x": -87344.3, "y": -331648.0, "type": "Connection"}, {"name": "Marcha Outpost", "x": -173259.0, "y": -447720.0, "type": "Gateway"}, {"name": "Gayak Altar", "x": -128428.0, "y": -443059.0, "type": "Connection"}, {"name": "Fountain of Origin", "x": -152817.0, "y": -315614.0, "type": "Connection"}, {"name": "Gervish Mountains", "x": 13771.6, "y": -285228.0, "type": "Connection"}, {"name": "Dormann Lumber Camp", "x": -35312.8, "y": -226421.0, "type": "Connection"}, {"name": "Khimut Lumber Camp", "x": 109532.0, "y": -209077.0, "type": "Connection"}, {"name": "Forgotten Gateway", "x": 127665.0, "y": -260158.0, "type": "Gateway"}, {"name": "Tshira Ruins", "x": 104760.0, "y": -273497.0, "type": "Connection"}, {"name": "Blood Wolf Settlement", "x": 68241.7, "y": -345612.0, "type": "Dangerous"}, {"name": "Marak Farm", "x": 2193.15, "y": -397162.0, "type": "Connection"}, {"name": "Farming", "x": -79019.3, "y": -421166.0, "type": ""}, {"name": "Farming", "x": -73583.8, "y": -429092.0, "type": ""}, {"name": "Farming", "x": 2660.57, "y": -400004.0, "type": ""}, {"name": "Farming", "x": -2349.13, "y": -395311.0, "type": ""}, {"name": "Gathering", "x": 30481.1, "y": -279990.0, "type": ""}, {"name": "Gathering", "x": 13926.9, "y": -281193.0, "type": ""}, {"name": "Gathering", "x": 110155.0, "y": -276132.0, "type": ""}, {"name": "Gathering", "x": 104424.0, "y": -278819.0, "type": ""}, {"name": "Mining", "x": -231580.0, "y": -379636.0, "type": ""}, {"name": "Mining", "x": -229705.0, "y": -392450.0, "type": ""}, {"name": "Mining", "x": -207233.0, "y": -423615.0, "type": ""}, {"name": "Mining", "x": -212604.0, "y": -418018.0, "type": ""}, {"name": "Lumbering", "x": -33772.0, "y": -222245.0, "type": ""}, {"name": "Lumbering", "x": -36854.3, "y": -224821.0, "type": ""}, {"name": "Lumbering", "x": 108080.0, "y": -211543.0, "type": ""}, {"name": "Lumbering", "x": 108973.0, "y": -206176.0, "type": ""}, {"name": "Excavation", "x": -149829.0, "y": -360678.0, "type": ""}, {"name": "Excavation", "x": -154723.0, "y": -301268.0, "type": ""}, {"name": "Investment Bank", "x": -47058.1, "y": -404705.0, "type": ""}, {"name": "Investment Bank", "x": -55153.1, "y": -416013.0, "type": ""}, {"name": "O'draxxia", "x": -156718.0, "y": -598990.0, "type": "City"}, {"name": "O'dyllita Castle", "x": -228034.0, "y": -636031.0, "type": "Connection"}, {"name": "Thornwood Forest", "x": -287499.0, "y": -497683.0, "type": "Connection"}, {"name": "Crypt of Resting Thoughts", "x": -234142.0, "y": -521656.0, "type": "Connection"}, {"name": "Narcion", "x": -261485.0, "y": -594713.0, "type": "Connection"}, {"name": "Talibahr's Rope", "x": -360344.0, "y": -568945.0, "type": "Connection"}, {"name": "La O'delle", "x": -308707.0, "y": -601945.0, "type": "Connection"}, {"name": "Starry Midnight Port", "x": -321451.0, "y": -598642.0, "type": "Connection"}, {"name": "O'dyllita Castle Vicinity", "x": -304406.0, "y": -626525.0, "type": "Connection"}, {"name": "Tunkuta", "x": -316716.0, "y": -518102.0, "type": "Connection"}, {"name": "Salun's Border", "x": -316109.0, "y": -486654.0, "type": "Connection"}, {"name": "Bahit Sanctum", "x": -210134.0, "y": -481296.0, "type": "Connection"}, {"name": "Olun's Valley", "x": -155956.0, "y": -523272.0, "type": "Connection"}, {"name": "Star's End", "x": -498873.0, "y": -67266.2, "type": "Dangerous"}, {"name": "Calpheon Northwestern Outpost", "x": -443854.0, "y": 1307.62, "type": "Gateway"}, {"name": "UnKnown", "x": -122686.0, "y": -657172.0, "type": ""}, {"name": "Grape Farming", "x": -184287.0, "y": -572593.0, "type": ""}, {"name": "Potato Farming", "x": -184591.0, "y": -561201.0, "type": ""}, {"name": "Excavation", "x": -231647.0, "y": -520579.0, "type": ""}, {"name": "Mining", "x": -167812.0, "y": -515349.0, "type": ""}, {"name": "Mining", "x": -162101.0, "y": -529460.0, "type": ""}, {"name": "Lumbering", "x": -292744.0, "y": -520328.0, "type": ""}, {"name": "Chicken Meat Production", "x": -268688.0, "y": -597347.0, "type": ""}, {"name": "Fish Drying Yard", "x": -320454.0, "y": -609989.0, "type": ""}, {"name": "Lumbering", "x": -206628.0, "y": -602447.0, "type": ""}, {"name": "Excavation", "x": -212683.0, "y": -458669.0, "type": ""}, {"name": "Mining", "x": -501215.0, "y": -71997.2, "type": ""}, {"name": "Brellin Farm", "x": -409482.0, "y": 19615.4, "type": "Connection"}, {"name": "Outpost Supply Port", "x": -448729.0, "y": 27309.0, "type": "Connection"}, {"name": "Excavation", "x": -498240.0, "y": -65607.5, "type": ""}, {"name": "NOT_A_NODE", "x": -1018380.0, "y": 1043240.0, "type": ""}, {"name": "NOT_A_NODE", "x": -709930.0, "y": 1231870.0, "type": ""}, {"name": "NOT_A_NODE", "x": -632996.0, "y": 1132940.0, "type": ""}, {"name": "NOT_A_NODE", "x": -337874.0, "y": 1158330.0, "type": ""}, {"name": "NOT_A_NODE", "x": -517383.0, "y": 862095.0, "type": ""}, {"name": "NOT_A_NODE", "x": -816088.0, "y": 669021.0, "type": ""}, {"name": "Oquilla's Eye", "x": -87328.4, "y": 626901.0, "type": "City"}, {"name": "NOT_A_NODE", "x": -748316.0, "y": 505615.0, "type": ""}, {"name": "NOT_A_NODE", "x": -940162.0, "y": 556455.0, "type": ""}, {"name": "NOT_A_NODE", "x": -864896.0, "y": 1081480.0, "type": ""}, {"name": "NOT_A_NODE", "x": -660737.0, "y": 799925.0, "type": ""}, {"name": "NOT_A_NODE", "x": -850719.0, "y": 890429.0, "type": ""}, {"name": "NOT_A_NODE", "x": -543161.0, "y": 1030450.0, "type": ""}, {"name": "Chiro's Cannon Workshop", "x": 38344.9, "y": 379244.0, "type": ""}, {"name": "Chiro's Sail Workshop", "x": 65814.8, "y": 408081.0, "type": ""}, {"name": "Chiro's Figurehead Workshop", "x": 222464.0, "y": 552340.0, "type": ""}, {"name": "Chiro's Black Plating Workshop", "x": 266513.0, "y": 539703.0, "type": ""}, {"name": "Grándiha", "x": -559743.0, "y": -476904.0, "type": "Trading Post"}, {"name": "Papua Crinea", "x": -677907.0, "y": -185590.0, "type": "Trading Post"}, {"name": "Forgotten Mountain", "x": -151295.0, "y": -542246.0, "type": "Connection"}, {"name": "Salanar Pond", "x": -211823.0, "y": -570860.0, "type": "Connection"}, {"name": "Delmira Plantation", "x": -182573.0, "y": -566774.0, "type": "Connection"}, {"name": "Mountain of Division", "x": -223470.0, "y": -450712.0, "type": "Connection"}, {"name": "Shiv Valley Road", "x": -190238.0, "y": -611333.0, "type": "Connection"}, {"name": "UnKnown", "x": 261650.0, "y": 611895.0, "type": ""}, {"name": "Crow's Nest", "x": 237202.0, "y": 696234.0, "type": "Connection"}, {"name": "Awina's Tail", "x": 177723.0, "y": -291551.0, "type": "Connection"}, {"name": "Wind Nol's Perch", "x": 166645.0, "y": -344640.0, "type": "Connection"}, {"name": "Erethea's Belt", "x": 132159.0, "y": -375514.0, "type": "Connection"}, {"name": "Eilton", "x": 170079.0, "y": -398472.0, "type": "City"}, {"name": "Maslan's Yulas Citron Orchard", "x": 210846.0, "y": -392860.0, "type": "Connection"}, {"name": "Snowstorm Guard Post", "x": 202795.0, "y": -426423.0, "type": "Connection"}, {"name": "Mountain of Eternal Winter", "x": 190969.0, "y": -497196.0, "type": "Connection"}, {"name": "Bronte's Bolt", "x": 160616.0, "y": -455732.0, "type": "Connection"}, {"name": "Jade Starlight Forest", "x": 151137.0, "y": -494401.0, "type": "Connection"}, {"name": "Mountain Top Guard Post", "x": 126765.0, "y": -470721.0, "type": "Connection"}, {"name": "Tori Woods", "x": 108526.0, "y": -506441.0, "type": "Connection"}, {"name": "Sherekhan Iron Mine", "x": 46503.2, "y": -503673.0, "type": "Connection"}, {"name": "Derelict Trade Post", "x": 27641.7, "y": -480908.0, "type": "Connection"}, {"name": "Shrine of Silent Prayers", "x": 7380.07, "y": -478244.0, "type": "Connection"}, {"name": "Zvier Highlands", "x": 112707.0, "y": -441773.0, "type": "Connection"}, {"name": "Camp Balacs", "x": 88542.3, "y": -434930.0, "type": "Connection"}, {"name": "Charbonneau Villa", "x": 94034.1, "y": -403331.0, "type": "Connection"}, {"name": "Pilgrim's End", "x": -51157.5, "y": -524862.0, "type": "Connection"}, {"name": "Mining", "x": 184885.0, "y": -506317.0, "type": ""}, {"name": "Gathering", "x": 212423.0, "y": -393968.0, "type": ""}, {"name": "Mining", "x": 47789.2, "y": -498102.0, "type": ""}, {"name": "Excavation", "x": 44316.4, "y": -495428.0, "type": ""}, {"name": "Lumbering", "x": 148872.0, "y": -487235.0, "type": ""}, {"name": "Mining", "x": 155524.0, "y": -494690.0, "type": ""}, {"name": "Gathering", "x": -57294.4, "y": -526564.0, "type": ""}, {"name": "Gathering", "x": -42912.7, "y": -527360.0, "type": ""}, {"name": "Excavation", "x": 89848.0, "y": -439017.0, "type": ""}, {"name": "Gathering", "x": 105974.0, "y": -509365.0, "type": ""}, {"name": "Lumbering", "x": 132240.0, "y": -466935.0, "type": ""}, {"name": "Excavation", "x": 108687.0, "y": -445969.0, "type": ""}, {"name": "Honey Production", "x": 109134.0, "y": -437934.0, "type": ""}, {"name": "Lumbering", "x": 169259.0, "y": -347071.0, "type": ""}, {"name": "Dalbeol Village", "x": -1130090.0, "y": 1271810.0, "type": "City"}, {"name": "Hanji County", "x": -1324610.0, "y": 1242840.0, "type": "Connection"}, {"name": "Shimnidae Forest", "x": -1296750.0, "y": 1176650.0, "type": "Connection"}, {"name": "Nampo Gate", "x": -1346900.0, "y": 1165660.0, "type": "Connection"}, {"name": "Nampo's Moodle Village", "x": -1312270.0, "y": 1136060.0, "type": "City"}, {"name": "Solgaji Forest", "x": -1253520.0, "y": 1183370.0, "type": "Connection"}, {"name": "Cheongsan Institute", "x": -1208110.0, "y": 1148140.0, "type": "Connection"}, {"name": "Dokkebi Forest", "x": -1215750.0, "y": 1258110.0, "type": "Connection"}, {"name": "Golden Pig Cave", "x": -1146310.0, "y": 1153160.0, "type": "Connection"}, {"name": "Gowun Plateau", "x": -1162420.0, "y": 1191470.0, "type": "Connection"}, {"name": "Drybranch Village", "x": -1119640.0, "y": 1138390.0, "type": "Connection"}, {"name": "Bomnae County", "x": -1069940.0, "y": 1122840.0, "type": "Connection"}, {"name": "Honglim Base", "x": -1084170.0, "y": 1198440.0, "type": "Connection"}, {"name": "Yeowoo Pass", "x": -1079590.0, "y": 1239720.0, "type": "Connection"}, {"name": "Nopsae's Byeot County", "x": -1031140.0, "y": 1298580.0, "type": "City"}, {"name": "Bari Forest", "x": -1105830.0, "y": 1421990.0, "type": "Connection"}, {"name": "Beombawi Valley", "x": -1199430.0, "y": 1387610.0, "type": "Connection"}, {"name": "Beombawi Gate", "x": -1214200.0, "y": 1439970.0, "type": "Connection"}, {"name": "Haemo Island", "x": -1384770.0, "y": 1006880.0, "type": "Connection"}, {"name": "Byukgye Island", "x": -1229970.0, "y": 1073860.0, "type": "Connection"}, {"name": "UnKnown", "x": -1301640.0, "y": 1179810.0, "type": ""}, {"name": "UnKnown", "x": -1293990.0, "y": 1180730.0, "type": ""}, {"name": "UnKnown", "x": -1249610.0, "y": 1177640.0, "type": ""}, {"name": "UnKnown", "x": -1217180.0, "y": 1260100.0, "type": ""}, {"name": "UnKnown", "x": -1212290.0, "y": 1255330.0, "type": ""}, {"name": "UnKnown", "x": -1144570.0, "y": 1152520.0, "type": ""}, {"name": "UnKnown", "x": -1164240.0, "y": 1193780.0, "type": ""}, {"name": "UnKnown", "x": -1160130.0, "y": 1190220.0, "type": ""}, {"name": "UnKnown", "x": -1070030.0, "y": 1125080.0, "type": ""}, {"name": "UnKnown", "x": -1065190.0, "y": 1122260.0, "type": ""}, {"name": "UnKnown", "x": -1085110.0, "y": 1201270.0, "type": ""}, {"name": "UnKnown", "x": -1082560.0, "y": 1195180.0, "type": ""}, {"name": "UnKnown", "x": -1084960.0, "y": 1239460.0, "type": ""}, {"name": "UnKnown", "x": -1075410.0, "y": 1237370.0, "type": ""}, {"name": "UnKnown", "x": -1109500.0, "y": 1427000.0, "type": ""}, {"name": "UnKnown", "x": -1102560.0, "y": 1418170.0, "type": ""}, {"name": "UnKnown", "x": -1202700.0, "y": 1385620.0, "type": ""}, {"name": "UnKnown", "x": -1194070.0, "y": 1383800.0, "type": ""}, {"name": "UnKnown", "x": -1388680.0, "y": 1007400.0, "type": ""}, {"name": "UnKnown", "x": -1317030.0, "y": 1247080.0, "type": ""}, {"name": "UnKnown", "x": -1325270.0, "y": 1239910.0, "type": ""}, {"name": "UnKnown", "x": -1020120.0, "y": 1285580.0, "type": ""}, {"name": "UnKnown", "x": -1022980.0, "y": 1300010.0, "type": ""}, {"name": "UnKnown", "x": -1020000.0, "y": 1301300.0, "type": ""}, {"name": "UnKnown", "x": -1006000.0, "y": 1294740.0, "type": ""}, {"name": "UnKnown", "x": -1195760.0, "y": 1391440.0, "type": ""}, {"name": "UnKnown", "x": -1195760.0, "y": 1391440.0, "type": ""}, {"name": "Dallae Pier", "x": -994463.0, "y": 1341100.0, "type": "Connection"}, {"name": "Dallaer Pier Quarry", "x": -1002650.0, "y": 1340760.0, "type": ""}, {"name": "Asparkan", "x": 278339.0, "y": -196126.0, "type": "City"}, {"name": "Atessahra", "x": 333786.0, "y": -223634.0, "type": "Connection"}, {"name": "Sezec Mercenary Camp", "x": 354493.0, "y": -263311.0, "type": "Connection"}, {"name": "City of the Dead", "x": 281446.0, "y": -286492.0, "type": "Connection"}, {"name": "Tungrad Ruins", "x": 443466.0, "y": -256073.0, "type": "Connection"}, {"name": "UnKnown", "x": 40373.9, "y": 83345.7, "type": ""}, {"name": "Neruda Plain", "x": 286823.0, "y": -224897.0, "type": "Connection"}, {"name": "Tremorin Hill", "x": 329799.0, "y": -277248.0, "type": "Connection"}, {"name": "Kermelun Wilds", "x": 402192.0, "y": -212027.0, "type": "Connection"}, {"name": "Muzgar", "x": 360002.0, "y": -317810.0, "type": "Town"}, {"name": "Aakshrad Mountains", "x": 301723.0, "y": -352575.0, "type": "Connection"}, {"name": "Yzrahid Highlands", "x": 301014.0, "y": -399669.0, "type": "Connection"}, {"name": "Darkseekers' Retreat", "x": 391831.0, "y": -284476.0, "type": "Connection"}, {"name": "Karasi Canyon", "x": 447929.0, "y": -315341.0, "type": "Connection"}, {"name": "Barhan Camp", "x": 500343.0, "y": -307967.0, "type": "Connection"}, {"name": "Shakhtar Wilds", "x": 411694.0, "y": -371751.0, "type": "Connection"}, {"name": "Velandir", "x": 386345.0, "y": -391883.0, "type": "Town"}, {"name": "Stofbir", "x": 483897.0, "y": -391364.0, "type": "Connection"}, {"name": "Seoul", "x": -1419520.0, "y": 1333790.0, "type": "City"}, {"name": "Yukjo Street", "x": -1472040.0, "y": 1337990.0, "type": "Town"}, {"name": "Unjongga Street", "x": -1411430.0, "y": 1266690.0, "type": "Connection"}, {"name": "Yuunru", "x": -1464560.0, "y": 1301360.0, "type": "Connection"}, {"name": "Chowon", "x": -1446800.0, "y": 1267550.0, "type": "Connection"}, {"name": "Godu Village", "x": -1419150.0, "y": 1239190.0, "type": "Town"}, {"name": "Bukpo", "x": -1342900.0, "y": 1509910.0, "type": "Town"}, {"name": "Won Jingung", "x": -1496200.0, "y": 1290800.0, "type": "Connection"}, {"name": "Myeonggyunjeon", "x": -1386780.0, "y": 1393900.0, "type": "Connection"}, {"name": "Milbon", "x": -1366160.0, "y": 1303670.0, "type": "Connection"}, {"name": "Taehak", "x": -1390910.0, "y": 1203740.0, "type": "Connection"}, {"name": "Dangsup", "x": -1519790.0, "y": 1245610.0, "type": "Connection"}, {"name": "Geogugoegul", "x": -1481220.0, "y": 1226490.0, "type": "Connection"}, {"name": "Holbon", "x": -1519330.0, "y": 1126250.0, "type": "Connection"}, {"name": "Guleumnalu", "x": -1520090.0, "y": 1099110.0, "type": "Connection"}, {"name": "Hwaseongok", "x": -1460000.0, "y": 1152100.0, "type": "Connection"}, {"name": "Dumegol", "x": -1437540.0, "y": 1140380.0, "type": "Connection"}, {"name": "Mongryong's Exile", "x": -1457070.0, "y": 1106210.0, "type": "Connection"}, {"name": "Asisan", "x": -1344890.0, "y": 1436350.0, "type": "Connection"}, {"name": "Cheonjedan", "x": -1319890.0, "y": 1445670.0, "type": "Connection"}, {"name": "Musinje", "x": -1270170.0, "y": 1508030.0, "type": "Connection"}, {"name": "Seryeondang", "x": -1351910.0, "y": 1476820.0, "type": "Connection"}, {"name": "Byeolli Forest", "x": -1425470.0, "y": 1456950.0, "type": "Connection"}, {"name": "Motgol Village", "x": -1389700.0, "y": 1459890.0, "type": "Connection"}, {"name": "Jamhwa Swamp", "x": -1460010.0, "y": 1419360.0, "type": "Connection"}, {"name": "Dongmakgol", "x": -1511210.0, "y": 1375730.0, "type": "Connection"}, {"name": "Deungryong Cave", "x": -1505690.0, "y": 1357350.0, "type": "Connection"}, {"name": "UnKnown", "x": -1428600.0, "y": 1231620.0, "type": ""}, {"name": "UnKnown", "x": -1427190.0, "y": 1239000.0, "type": "Connection"}, {"name": "UnKnown", "x": -1434560.0, "y": 1234670.0, "type": "Connection"}, {"name": "UnKnown", "x": -1429670.0, "y": 1244740.0, "type": "Connection"}, {"name": "UnKnown", "x": -1502520.0, "y": 1283440.0, "type": "Connection"}, {"name": "UnKnown", "x": -1382540.0, "y": 1400410.0, "type": "Connection"}, {"name": "UnKnown", "x": -1354360.0, "y": 1303620.0, "type": "Connection"}, {"name": "UnKnown", "x": -1419000.0, "y": 1265230.0, "type": "Connection"}, {"name": "UnKnown", "x": -1467410.0, "y": 1231830.0, "type": "Connection"}, {"name": "UnKnown", "x": -1470270.0, "y": 1228870.0, "type": "Connection"}, {"name": "UnKnown", "x": -1432640.0, "y": 1143490.0, "type": "Connection"}, {"name": "UnKnown", "x": -1352780.0, "y": 1439840.0, "type": "Connection"}, {"name": "UnKnown", "x": -1433740.0, "y": 1456300.0, "type": "Connection"}, {"name": "UnKnown", "x": -1429190.0, "y": 1463290.0, "type": "Connection"}, {"name": "UnKnown", "x": -1382920.0, "y": 1461770.0, "type": "Connection"}, {"name": "UnKnown", "x": -1507050.0, "y": 1371290.0, "type": "Connection"}, {"name": "UnKnown", "x": -1501130.0, "y": 1351870.0, "type": "Connection"}, {"name": "UnKnown", "x": -1504620.0, "y": 1351700.0, "type": "Connection"}, {"name": "UnKnown", "x": -1522290.0, "y": 1233090.0, "type": "Connection"}, {"name": "Hakinza Sanctuary", "x": 537251.0, "y": 477184.0, "type": "City"}, {"name": "Aetherion Castle", "x": 559135.0, "y": 582598.0, "type": "Castle"}, {"name": "Orbita Castle", "x": 671762.0, "y": 486041.0, "type": "Castle"}, {"name": "Zephyros Castle", "x": 723838.0, "y": 620540.0, "type": "Castle"}, {"name": "Tenebraum Castle", "x": 661604.0, "y": 688745.0, "type": "Castle"}, {"name": "Nymphamaré Castle", "x": 529002.0, "y": 726282.0, "type": "Castle"}, {"name": "Escar Mountains", "x": 530620.0, "y": 405888.0, "type": "Connection"}, {"name": "Shore of Ruins", "x": 514502.0, "y": 426607.0, "type": "Connection"}, {"name": "Neftak Outpost", "x": 539132.0, "y": 505851.0, "type": "Connection"}, {"name": "Faith's Resting Place", "x": 631103.0, "y": 532538.0, "type": "Connection"}, {"name": "Great White Spot", "x": 590001.0, "y": 485581.0, "type": "Connection"}, {"name": "Sanctified Mercy", "x": 674483.0, "y": 471061.0, "type": "Connection"}, {"name": "Litovan Mountains", "x": 720240.0, "y": 510445.0, "type": "Connection"}, {"name": "Azure Battlefield", "x": 583383.0, "y": 622398.0, "type": "Connection"}, {"name": "Saterna Mountains", "x": 618669.0, "y": 556623.0, "type": "Connection"}, {"name": "Urnas Mountains", "x": 488923.0, "y": 577358.0, "type": "Connection"}, {"name": "Aal's Revelation", "x": 485929.0, "y": 548987.0, "type": "Connection"}, {"name": "Cliff of Despair", "x": 515719.0, "y": 608881.0, "type": "Connection"}, {"name": "Euphetar Mountains", "x": 630105.0, "y": 567492.0, "type": "Connection"}, {"name": "The Canted Ring", "x": 540931.0, "y": 643414.0, "type": "Connection"}, {"name": "Stillcoral Grove", "x": 516093.0, "y": 679670.0, "type": "Connection"}, {"name": "Great Dark Spot", "x": 506975.0, "y": 723093.0, "type": "Connection"}, {"name": "Tideworn Gorge", "x": 511263.0, "y": 776961.0, "type": "Connection"}, {"name": "Mount Rumanaré", "x": 578142.0, "y": 722566.0, "type": "Connection"}, {"name": "Whispering Hills", "x": 556843.0, "y": 680902.0, "type": "Connection"}, {"name": "Wailing Altar", "x": 627938.0, "y": 683775.0, "type": "Connection"}, {"name": "Garden of Immortality", "x": 685615.0, "y": 660572.0, "type": "Connection"}, {"name": "Veiled Archives", "x": 656722.0, "y": 651331.0, "type": "Connection"}, {"name": "Crossroads of Defiance", "x": 646483.0, "y": 611560.0, "type": "Connection"}, {"name": "Ancient Ruins", "x": 693631.0, "y": 578965.0, "type": "Connection"}, {"name": "Scorched Land of Prophecy", "x": 693749.0, "y": 534820.0, "type": "Connection"}, {"name": "Great Red Spot", "x": 670139.0, "y": 547956.0, "type": "Connection"}, {"name": "Doomstill Pond", "x": 735589.0, "y": 564719.0, "type": "Connection"}, {"name": "Ebony Opening", "x": 612647.0, "y": 643326.0, "type": "Connection"}, {"name": "Platerra Mountains", "x": 609412.0, "y": 699216.0, "type": "Connection"}, {"name": "Sanctuary Coastal Outpost", "x": 516022.0, "y": 460246.0, "type": "Connection"}];
const TRADE_MANAGERS = [{"node": "Velia", "npc": "Bahar"}, {"node": "Bartali Farm", "npc": "Emma Bartali"}, {"node": "Western Guard Camp", "npc": "Luke"}, {"node": "Finto Farm", "npc": "Martina Finto"}, {"node": "Balenos Forest", "npc": "Daphne DelLucci"}, {"node": "Loggia Farm", "npc": "Severo Loggia"}, {"node": "Toscani Farm", "npc": "Ovidio Toscani"}, {"node": "Marino Farm", "npc": "Rovant Marino"}, {"node": "Olvia", "npc": "Lolly"}, {"node": "Heidel", "npc": "Siuta"}, {"node": "Glish", "npc": "Larc"}, {"node": "Central Guard Camp", "npc": "Trade Manager Xenians"}, {"node": "Southern Guard Camp", "npc": "Trade Manager Anti"}, {"node": "Moretti Plantation", "npc": "Mercianne Moretti"}, {"node": "Alejandro Farm", "npc": "Amadeo Alejandro"}, {"node": "Elda Farm", "npc": "Coco Elda"}, {"node": "Northwestern Gateway", "npc": "Trade Manager Ginta"}, {"node": "Southwestern Gateway", "npc": "Trade Manager Theonil"}, {"node": "Eastern Gateway", "npc": "Breman"}, {"node": "Costa Farm", "npc": "Mael Costa"}, {"node": "Lynch Ranch", "npc": "Murana Lynch"}, {"node": "Keplan", "npc": "Hamir"}, {"node": "Heidel Pass", "npc": "Trade Manager Kirklas"}, {"node": "Calpheon Slum Trade Zone", "npc": "Harden"}, {"node": "Calpheon Market Trade Zone", "npc": "Lindsiyana Herba"}, {"node": "Calpheon Holy College Trade Zone", "npc": "Wolfgang"}, {"node": "Florin", "npc": "Trade Manager Loria"}, {"node": "Port Epheria", "npc": "Trade Manager Olivino Grolin"}, {"node": "Trent", "npc": "Rikta"}, {"node": "Behr", "npc": "Triee"}, {"node": "Crioville", "npc": "Herio"}, {"node": "Longleaf Tree Sentry Post", "npc": "Trade Manager Koirin"}, {"node": "Contaminated Farm", "npc": "Libero"}, {"node": "Dias Farm", "npc": "Enzo"}, {"node": "Cohen Farm", "npc": "Jacob"}, {"node": "Bernianto Farm", "npc": "Griffian Bernianto"}, {"node": "Marni Cave Path", "npc": "Henge Bato"}, {"node": "Falres Dirt Farm", "npc": "Jame Falres"}, {"node": "Bain Farmland", "npc": "Ann"}, {"node": "Oberen Farm", "npc": "Matheo Oberen"}, {"node": "Beacon Entrance Post", "npc": "Lonebaer"}, {"node": "Abandoned Quarry", "npc": "Abandoned Quarry Scout Theo"}, {"node": "Gianin Farm", "npc": "Goolie Gianin"}, {"node": "Serendia Western Gateway", "npc": "Batuetta"}, {"node": "Oze Pass", "npc": "Rock Investigator Enruka"}, {"node": "Hill Path", "npc": "Stranded Soldier John"}, {"node": "Eberdeen Island", "npc": "Merio"}, {"node": "Rhutum Sentry Post", "npc": "Elinke Visamin"}, {"node": "Abandoned Monastery", "npc": "Trade Manager Bacho Ladericcio"}, {"node": "South Kaia Pier", "npc": "Bavao"}, {"node": "Gabino Farm", "npc": "Bob Anderson"}, {"node": "Mansha Forest", "npc": "Mansha"}, {"node": "Tobare's Cabin", "npc": "Tobare"}, {"node": "Mediah Northern Gateway", "npc": "Suna Lise"}, {"node": "Omar Lava Cave", "npc": "Hakan Derk"}, {"node": "Stonetail Horse Ranch", "npc": "Asran"}, {"node": "Shuri Farm", "npc": "Anna Marre"}, {"node": "Kasula Farm", "npc": "Zaramas Kasula"}, {"node": "Delphe Knights Castle", "npc": "Granbill"}, {"node": "Anti-Troll Fortification", "npc": "Andre Vidal"}, {"node": "Phoniel's Cabin", "npc": "Villa Owner Phoniel"}, {"node": "Northern Wheat Plantation", "npc": "Norma Leight"}, {"node": "Delphe Outpost", "npc": "Trade Manager Raibo"}, {"node": "Iliya Island", "npc": "Trade Manager Maonil"}, {"node": "Altinova", "npc": "Quina"}, {"node": "Tarif", "npc": "Brorum"}, {"node": "Abun", "npc": "Trade Manager Kesir Baum"}, {"node": "Kusha", "npc": "Chakra"}, {"node": "Splashing Point", "npc": "Trade Manager Tacho"}, {"node": "Pilava Island", "npc": "Gerold"}, {"node": "Racid Island", "npc": "Kunka"}, {"node": "Baremi Island", "npc": "Sidimin"}, {"node": "Beiruwa Island", "npc": "Isaria"}, {"node": "Luivano Island", "npc": "Izaak"}, {"node": "Duch Island", "npc": "Andes"}, {"node": "Louruve Island", "npc": "Bilao"}, {"node": "Albresser Island", "npc": "Ninehart"}, {"node": "Daton Island", "npc": "Sion"}, {"node": "Randis Island", "npc": "Sagotts"}, {"node": "Theonil Island", "npc": "Riotina"}, {"node": "Deserted City of Runn", "npc": "Tony Vangertz"}, {"node": "Ancado Inner Harbor", "npc": "Inaha"}, {"node": "Orisha Island", "npc": "Seltin"}, {"node": "Valencia City", "npc": "Yis Kunjamin"}, {"node": "Valencia City", "npc": "Burita Allon"}, {"node": "Sand Grain Bazaar", "npc": "Atui Balacs"}, {"node": "Shakatu", "npc": "Taphtar"}, {"node": "Rock Post", "npc": "Trade Manager Siamak"}, {"node": "Ibellab Oasis", "npc": "Trade Manager Shuriar"}, {"node": "Arehaza", "npc": "Surondula"}, {"node": "Muiquun", "npc": "Trade Manager Sophia"}, {"node": "Lemoria Guard Post", "npc": "Trade Manager Leminei Lain"}, {"node": "Old Wisdom Tree", "npc": "Obi Bellen"}, {"node": "Viv Foretta Hamlet", "npc": "Trade Manager Norn Federers"}, {"node": "Grána", "npc": "Okiara"}, {"node": "Lake Flondor", "npc": "Trade Manager Maina"}, {"node": "Acher Guard Post", "npc": "Munanslyn"}, {"node": "Tooth Fairy Cabin", "npc": "Trade Manager Bronn"}, {"node": "Ash Forest", "npc": "Ashlynn"}, {"node": "Altinova", "npc": "Nyabee"}, {"node": "Duvencrune Farmland", "npc": "Dostter"}, {"node": "Sherekhan Necropolis", "npc": "Camira"}, {"node": "Ahib Conflict Zone", "npc": "Selena Aer"}, {"node": "Marcha Outpost", "npc": "Ladar"}, {"node": "Khimut Lumber Camp", "npc": "Karl Verdun"}, {"node": "Dormann Lumber Camp", "npc": "Dormann"}, {"node": "Duvencrune Farmland", "npc": "Tikara"}, {"node": "Grándiha", "npc": "Titu"}, {"node": "Papua Crinea", "npc": "Benns Lamute"}, {"node": "Eilton", "npc": "Bollona"}, {"node": "Camp Balacs", "npc": "Jorg"}, {"node": "Awina's Tail", "npc": "Huan"}, {"node": "Pilgrim's End", "npc": "Lisae"}, {"node": "Nampo's Moodle Village", "npc": "Gapsam"}, {"node": "Dalbeol Village", "npc": "Youngim"}, {"node": "Nopsae's Byeot County", "npc": "Old Lady Bokdeok"}, {"node": "Asparkan", "npc": "Ametullah"}, {"node": "Muzgar", "npc": "Sunnak"}, {"node": "Unjongga Street", "npc": "Gyeonghwan"}, {"node": "Seoul", "npc": "Taesok (Yukjo NPC, Seoul Origin)"}, {"node": "Bukpo", "npc": "Chunsu"}, {"node": "Velandir", "npc": "Bahzam"}, {"node": "Oquilla's Eye", "npc": "Kario"}, {"node": "Hakinza Sanctuary", "npc": "Roig Mills"}];
const SCALE = 1470588;

const DIST_CAP = 150;      // distance bonus cap (%)
const TRADER_BONUS = 30;   // always-on trader bonus (%)

// Continuous "total level" mapping (Master 1-30, Guru 1-50)
const RANK_OFFSETS = {
  "Beginner": 0,
  "Apprentice": 10,
  "Skilled": 20,
  "Professional": 30,
  "Artisan": 40,
  "Master": 50,
  "Guru": 80
};
const RANK_MAX = {
  "Beginner": 10,
  "Apprentice": 10,
  "Skilled": 10,
  "Professional": 10,
  "Artisan": 10,
  "Master": 30,
  "Guru": 50
};

const el = {
  originSearch: document.getElementById("originSearch"),
  originSelect: document.getElementById("originSelect"),
  fishPrice: document.getElementById("fishPrice"),
  tradeRank: document.getElementById("tradeRank"),
  tradeLevel: document.getElementById("tradeLevel"),
  applyBargain: document.getElementById("applyBargain"),
  bargainOut: document.getElementById("bargainOut"),
  bestOut: document.getElementById("bestOut"),
  bestDetails: document.getElementById("bestDetails"),
  saleOut: document.getElementById("saleOut"),
  saleDetails: document.getElementById("saleDetails"),
  rowsBody: document.getElementById("rowsBody"),
};

const THEME_OPTIONS = [
  { id: "ocean", label: "Ocean" },
  { id: "forest", label: "Forest" },
  { id: "sunset", label: "Sunset" },
  { id: "lavender", label: "Lavender" },
  { id: "slate", label: "Slate" },
  { id: "crimson", label: "Crimson" },
  { id: "gold", label: "Golden Hour" },
  { id: "midnight", label: "Midnight" },
];

const INTERFACE_PRESETS = {
  fantasy:   { mode:"dark", themeId:"gold",     strength:88, density:"comfortable", corners:"round", background:"embers" },
  cyber:     { mode:"dark", themeId:"lavender", strength:92, density:"compact",     corners:"sharp", background:"aurora" },
  cinematic: { mode:"dark", themeId:"slate",    strength:58, density:"comfortable", corners:"soft",  background:"stars" },
  crystal:   { mode:"dark", themeId:"ocean",    strength:86, density:"comfortable", corners:"round", background:"geometry" },
  tactical:  { mode:"dark", themeId:"forest",   strength:68, density:"compact",     corners:"sharp", background:"none" },
  retro:     { mode:"dark", themeId:"midnight", strength:82, density:"compact",     corners:"sharp", background:"geometry" },
  abyssal:   { mode:"dark", themeId:"ocean",    strength:88, density:"compact",     corners:"soft",  background:"ocean" },
  royal:     { mode:"dark", themeId:"crimson",  strength:80, density:"comfortable", corners:"round", background:"embers" },
  paper:     { mode:"light",themeId:"gold",     strength:48, density:"comfortable", corners:"soft",  background:"none" },
  foundry:   { mode:"dark", themeId:"sunset",   strength:86, density:"compact",     corners:"sharp", background:"embers" },
  void:      { mode:"dark", themeId:"lavender", strength:72, density:"compact",     corners:"sharp", background:"geometry" },
  caravan:   { mode:"light",themeId:"gold",     strength:58, density:"comfortable", corners:"round", background:"none" },
};

const appearanceEl = {
  modeToggle: document.getElementById("themeModeToggle"),
  modeLabel: document.getElementById("modeLabel"),
  themeChoices: [...document.querySelectorAll("[data-theme-choice]")],
  backgroundChoices: [...document.querySelectorAll("[data-background-choice]")],
  interfacePreviews: [...document.querySelectorAll("[data-interface-preview]")],
  interfaceStyle: document.getElementById("interfaceStyle"),
  backgroundStrength: document.getElementById("backgroundStrength"),
  strengthValue: document.getElementById("strengthValue"),
  density: document.getElementById("interfaceDensity"),
  corners: document.getElementById("cornerStyle"),
  reduceMotion: document.getElementById("reduceMotion"),
  toastEnabled: document.getElementById("toastNotificationsEnabled"),
  toastDuration: document.getElementById("toastDuration"),
  minimizeToTray: document.getElementById("minimizeToTrayEnabled"),
};

function loadAppearance() {
  try {
    return JSON.parse(localStorage.getItem("bdoTradeCalculatorAppearance") || "{}");
  } catch (_) {
    return {};
  }
}

function saveAppearance(settings) {
  try {
    localStorage.setItem("bdoTradeCalculatorAppearance", JSON.stringify(settings));
  } catch (_) {}
}

const settingMemory = new Map();
const settingWriteTimers = new Map();
function readSetting(key, fallback) {
  if(settingMemory.has(key)) return settingMemory.get(key);
  try {
    const value = localStorage.getItem(`bdoMultiTool.${key}`);
    const parsed = value ? JSON.parse(value) : fallback;
    settingMemory.set(key, parsed);
    return parsed;
  } catch (_) {
    settingMemory.set(key, fallback);
    return fallback;
  }
}

function flushSetting(key) {
  if(!settingMemory.has(key)) return;
  try { localStorage.setItem(`bdoMultiTool.${key}`, JSON.stringify(settingMemory.get(key))); } catch (_) {}
  clearTimeout(settingWriteTimers.get(key));
  settingWriteTimers.delete(key);
}

function persistSetting(key, value) {
  settingMemory.set(key, value);
  clearTimeout(settingWriteTimers.get(key));
  settingWriteTimers.set(key, setTimeout(() => flushSetting(key), 180));
}

window.addEventListener("pagehide", () => [...settingWriteTimers.keys()].forEach(flushSetting));

const NotificationService=(()=>{const host=document.getElementById("toastHost"),icons={success:"OK",error:"!",warning:"!",info:"i"},titles={success:"Success",error:"Error",warning:"Warning",info:"Info"};let enabled=true,duration=5000,lastKey="",lastShown=0;function configure(s={}){enabled=s.toastEnabled!==false;duration=Math.max(3000,Math.min(12000,Number(s.toastDuration||5)*1000))}function dismiss(t){if(!t||t.classList.contains("leaving"))return;t.classList.add("leaving");setTimeout(()=>t.remove(),190)}function show(type,message,title=titles[type]){if(!enabled||!host||!message)return null;const key=`${type}|${title}|${message}`,now=Date.now();if(key===lastKey&&now-lastShown<1200)return null;lastKey=key;lastShown=now;const t=document.createElement("div");t.className=`uiToast ${type}`;t.innerHTML=`<span class="toastIcon">${icons[type]}</span><span class="toastCopy"><strong>${escapeHtml(title)}</strong><span>${escapeHtml(message)}</span><small>${new Date().toLocaleTimeString([],{hour:"2-digit",minute:"2-digit"})}</small></span><button class="toastClose" type="button" aria-label="Close notification">&times;</button>`;t.querySelector(".toastClose").addEventListener("click",()=>dismiss(t));host.appendChild(t);setTimeout(()=>dismiss(t),type==="error"?Math.max(duration,8000):duration);return t}return{configure,ShowSuccess:(m,t)=>show("success",m,t),ShowError:(m,t)=>show("error",m,t),ShowWarning:(m,t)=>show("warning",m,t),ShowInfo:(m,t)=>show("info",m,t)}})();
const appConfirmEl={overlay:document.getElementById("appConfirmOverlay"),title:document.getElementById("appConfirmTitle"),message:document.getElementById("appConfirmMessage"),cancel:document.getElementById("appConfirmCancel"),accept:document.getElementById("appConfirmAccept")};
let appConfirmResolve=null,appConfirmReturnFocus=null;
function closeAppConfirm(result){
  if(!appConfirmResolve)return;
  const resolve=appConfirmResolve;
  appConfirmResolve=null;
  if(appConfirmEl.overlay)appConfirmEl.overlay.hidden=true;
  resolve(Boolean(result));
  if(appConfirmReturnFocus?.isConnected)setTimeout(()=>appConfirmReturnFocus.focus(),0);
  appConfirmReturnFocus=null;
}
function appConfirm(message,{title="Confirm action",acceptLabel="Delete"}={}){
  if(!appConfirmEl.overlay)return Promise.resolve(false);
  if(appConfirmResolve)closeAppConfirm(false);
  appConfirmReturnFocus=document.activeElement instanceof HTMLElement?document.activeElement:null;
  appConfirmEl.title.textContent=title;
  appConfirmEl.message.textContent=message;
  appConfirmEl.accept.textContent=acceptLabel;
  appConfirmEl.overlay.hidden=false;
  setTimeout(()=>appConfirmEl.cancel?.focus(),0);
  return new Promise(resolve=>{appConfirmResolve=resolve});
}
appConfirmEl.cancel?.addEventListener("click",()=>closeAppConfirm(false));
appConfirmEl.accept?.addEventListener("click",()=>closeAppConfirm(true));
appConfirmEl.overlay?.addEventListener("click",event=>{if(event.target===appConfirmEl.overlay)closeAppConfirm(false)});
appConfirmEl.overlay?.addEventListener("keydown",event=>{
  if(event.key==="Escape"){event.preventDefault();closeAppConfirm(false);return}
  if(event.key!=="Tab")return;
  const buttons=[appConfirmEl.cancel,appConfirmEl.accept].filter(Boolean);
  const first=buttons[0],last=buttons[buttons.length-1];
  if(event.shiftKey&&document.activeElement===first){event.preventDefault();last.focus()}
  else if(!event.shiftKey&&document.activeElement===last){event.preventDefault();first.focus()}
});

function applyAppearance(settings = {}) {
  if(settings.interfaceStyle === "celestial") settings = { ...settings, interfaceStyle:"fantasy" };
  const interfaceStyle = settings.interfaceStyle === "custom" || Object.hasOwn(INTERFACE_PRESETS, settings.interfaceStyle)
    ? settings.interfaceStyle : "fantasy";
  const customMode = interfaceStyle === "custom";
  const preset = customMode ? null : INTERFACE_PRESETS[interfaceStyle];
  const legacyIndex = Math.max(0, Math.min(THEME_OPTIONS.length - 1, Number(settings.themeIndex ?? 0)));
  const theme = customMode
    ? (THEME_OPTIONS.find(option => option.id === settings.themeId) || THEME_OPTIONS[legacyIndex])
    : THEME_OPTIONS.find(option => option.id === preset.themeId);
  const mode = customMode ? (settings.mode === "light" ? "light" : "dark") : preset.mode;
  const strength = customMode ? Math.max(20, Math.min(100, Number(settings.strength ?? 72))) : preset.strength;
  const density = customMode ? (settings.density === "compact" ? "compact" : "comfortable") : preset.density;
  const corners = customMode && ["soft","round","sharp"].includes(settings.corners) ? settings.corners : (customMode ? "soft" : preset.corners);
  const reducedMotion = settings.reducedMotion === true || window.matchMedia?.("(prefers-reduced-motion: reduce)").matches === true;
  const toastEnabled=settings.toastEnabled!==false;
  const toastDuration=[3,5,8,12].includes(Number(settings.toastDuration))?Number(settings.toastDuration):5;
  const background = customMode && ["none","aurora","stars","matrix","embers","ocean","geometry"].includes(settings.background)
    ? settings.background : (customMode ? "aurora" : preset.background);

  document.body.dataset.mode = mode;
  document.body.dataset.theme = theme.id;
  document.body.dataset.density = density;
  document.body.dataset.corners = corners;
  document.body.dataset.motion = reducedMotion ? "reduced" : "full";
  document.body.dataset.background = background;
  document.body.dataset.style = interfaceStyle;
  document.documentElement.style.setProperty("--bg-strength", String(strength / 100));

  if(appearanceEl.modeToggle) appearanceEl.modeToggle.checked = mode === "dark";
  if(appearanceEl.modeLabel) appearanceEl.modeLabel.textContent = mode === "dark" ? "Dark mode" : "Light mode";
  appearanceEl.themeChoices.forEach(button => button.classList.toggle("active", button.dataset.themeChoice === theme.id));
  appearanceEl.backgroundChoices.forEach(button => button.classList.toggle("active", button.dataset.backgroundChoice === background));
  appearanceEl.interfacePreviews.forEach(button => button.classList.toggle("active", button.dataset.interfacePreview === interfaceStyle));
  if(appearanceEl.backgroundStrength) appearanceEl.backgroundStrength.value = String(strength);
  if(appearanceEl.strengthValue) appearanceEl.strengthValue.textContent = `${strength}%`;
  if(appearanceEl.density) appearanceEl.density.value = density;
  if(appearanceEl.interfaceStyle) appearanceEl.interfaceStyle.value = interfaceStyle;
  if(appearanceEl.corners) appearanceEl.corners.value = corners;
  if(appearanceEl.reduceMotion) appearanceEl.reduceMotion.checked = reducedMotion;
  if(appearanceEl.toastEnabled) appearanceEl.toastEnabled.checked=toastEnabled;
  if(appearanceEl.toastDuration) appearanceEl.toastDuration.value=String(toastDuration);
  if(appearanceEl.modeToggle) appearanceEl.modeToggle.disabled = !customMode;
  if(appearanceEl.density) appearanceEl.density.disabled = !customMode;
  if(appearanceEl.corners) appearanceEl.corners.disabled = !customMode;
  if(appearanceEl.backgroundStrength) appearanceEl.backgroundStrength.disabled = !customMode;
  appearanceEl.themeChoices.forEach(button => button.disabled = !customMode);
  appearanceEl.backgroundChoices.forEach(button => button.disabled = !customMode);

  NotificationService.configure({toastEnabled,toastDuration});
  saveAppearance({ mode, themeId:theme.id, strength, density, corners, reducedMotion, background, interfaceStyle, toastEnabled, toastDuration });
}

const savedAppearance = loadAppearance();
applyAppearance({
  ...savedAppearance,
  reducedMotion: savedAppearance.reducedMotion === true,
  interfaceStyle: savedAppearance.interfaceStyle || "fantasy",
});

appearanceEl.modeToggle?.addEventListener("change", () => {
  const current = loadAppearance();
  applyAppearance({ ...current, mode: appearanceEl.modeToggle.checked ? "dark" : "light" });
});

appearanceEl.themeChoices.forEach(button => button.addEventListener("click", () => {
  const current = loadAppearance();
  applyAppearance({ ...current, themeId:button.dataset.themeChoice });
  NotificationService.ShowInfo(`${button.querySelector("strong")?.textContent||"Theme"} colors applied.`,"Theme changed");
}));

appearanceEl.backgroundChoices.forEach(button => button.addEventListener("click", () => {
  const current = loadAppearance();
  applyAppearance({ ...current, background:button.dataset.backgroundChoice });
  NotificationService.ShowInfo(`${button.querySelector("strong")?.textContent||"Background"} applied.`,"Theme changed");
}));

appearanceEl.interfaceStyle?.addEventListener("change", () => {
  const current = loadAppearance();
  applyAppearance({ ...current, interfaceStyle:appearanceEl.interfaceStyle.value });
  NotificationService.ShowInfo(`Interface changed to ${appearanceEl.interfaceStyle.selectedOptions[0].textContent}.`,"Theme changed");
});

appearanceEl.interfacePreviews.forEach(button => button.addEventListener("click", () => {
  const current = loadAppearance();
  const style = button.dataset.interfacePreview;
  applyAppearance({ ...current, interfaceStyle:style });
  NotificationService.ShowInfo(`${button.querySelector("strong")?.textContent||"Interface"} preview applied.`,"Theme changed");
}));

appearanceEl.density?.addEventListener("change", () => {
  const current = loadAppearance();
  applyAppearance({ ...current, density:appearanceEl.density.value });
});

appearanceEl.corners?.addEventListener("change", () => {
  const current = loadAppearance();
  applyAppearance({ ...current, corners:appearanceEl.corners.value });
});

appearanceEl.reduceMotion?.addEventListener("change", () => {
  const current = loadAppearance();
  applyAppearance({ ...current, reducedMotion:appearanceEl.reduceMotion.checked });
});
appearanceEl.toastEnabled?.addEventListener("change",()=>{const current=loadAppearance();applyAppearance({...current,toastEnabled:appearanceEl.toastEnabled.checked});if(appearanceEl.toastEnabled.checked)NotificationService.ShowInfo("Toast notifications are enabled.")});
appearanceEl.toastDuration?.addEventListener("change",()=>{const current=loadAppearance();applyAppearance({...current,toastDuration:Number(appearanceEl.toastDuration.value)});NotificationService.ShowInfo(`Toast duration set to ${appearanceEl.toastDuration.value} seconds.`)});
async function initializeAppBehaviorSettings(){try{const s=await bridgeCall("getAppBehaviorSettings");if(appearanceEl.minimizeToTray)appearanceEl.minimizeToTray.checked=s.minimizeToTray!==false;}catch{}}
appearanceEl.minimizeToTray?.addEventListener("change",async()=>{try{await bridgeCall("saveAppBehaviorSettings",{minimizeToTray:appearanceEl.minimizeToTray.checked});NotificationService.ShowInfo(`Close button will ${appearanceEl.minimizeToTray.checked?"minimize to tray":"close the app"}.`,"App behavior saved");}catch(error){NotificationService.ShowError(error.message||"Could not save app behavior.")}});
initializeAppBehaviorSettings();

appearanceEl.backgroundStrength?.addEventListener("input", () => {
  const current = loadAppearance();
  applyAppearance({ ...current, strength: Number(appearanceEl.backgroundStrength.value) });
});


function norm(s) {
  return (s||"").toString().normalize("NFD").replace(/[\u0300-\u036f]/g,"").toLowerCase().trim();
}
function escapeHtml(s) {
  return (s??"").toString().replace(/[&<>"']/g, c => ({"&":"&amp;","<":"&lt;",">":"&gt;",'"':"&quot;","'":"&#39;"}[c]));
}
function fmt(n, digits=2) {
  if(!Number.isFinite(n)) return "&mdash;";
  return n.toLocaleString(undefined, {maximumFractionDigits:digits, minimumFractionDigits:digits});
}
function fmtInt(n) {
  if(!Number.isFinite(n)) return "&mdash;";
  return Math.round(n).toLocaleString();
}

const nodesByName = new Map();
const ORIGIN_NODES = [];
const originNames = new Set();
for(const node of NODES) {
  const name = String(node.name || "").trim();
  const normalizedName = norm(name);
  if(name && !nodesByName.has(name)) nodesByName.set(name, node);
  if(!name || !node.type || normalizedName === "not_a_node" || normalizedName === "unknown" || originNames.has(normalizedName)) continue;
  originNames.add(normalizedName);
  ORIGIN_NODES.push(node);
}
const nodeSearchIndex = ORIGIN_NODES.map(node => ({node, search:norm(`${node.name} ${node.type||""}`)}));

const tmNpcByNode = new Map();
for(const t of TRADE_MANAGERS) {
  if(!tmNpcByNode.has(t.node)) tmNpcByNode.set(t.node, []);
  tmNpcByNode.get(t.node).push(t.npc);
}
const tmTargets = TRADE_MANAGERS.map(t => nodesByName.get(t.node)).filter(Boolean);
const seen = new Set();
const SELL_TARGETS = [];
for(const n of tmTargets) {
  if(seen.has(n.name)) continue;
  seen.add(n.name);
  SELL_TARGETS.push(n);
}

function clampLevel(rank, level) {
  const maxL = RANK_MAX[rank] ?? 10;
  let l = Number(level);
  if(!Number.isFinite(l) || l < 1) l = 1;
  if(l > maxL) l = maxL;
  return l;
}
function totalTradingLevel(rank, level) {
  const off = RANK_OFFSETS[rank] ?? 0;
  const l = clampLevel(rank, level);
  return off + l;
}
function bargainBonusPct(rank, level) {
  // Bonus% = 5 + 0.5 * totalLevel (per common bargain formula)
  return 5 + 0.5 * totalTradingLevel(rank, level);
}

function mapDistance(a,b) {
  return Math.hypot(a.x-b.x, a.y-b.y);
}
function distanceBonusPct(dist) {
  const pct = (dist / SCALE) * 100.0;
  return Math.max(0, Math.min(DIST_CAP, pct));
}

function refreshOriginMatches() {
  const q = norm(el.originSearch.value);
  const list = (q ? nodeSearchIndex.filter(entry => entry.search.includes(q)).map(entry => entry.node) : ORIGIN_NODES).slice(0, 300);

  el.originSelect.innerHTML = "";
  for(const n of list) {
    const opt = document.createElement("option");
    opt.value = n.name;
    opt.textContent = n.type ? `${n.name} - ${n.type}` : n.name;
    el.originSelect.appendChild(opt);
  }
  if(el.originSelect.options.length) el.originSelect.selectedIndex = 0;
}

function multiplier(distBonus, bargain) {
  const mDist = 1 + distBonus/100;
  const mBarg = 1 + bargain/100;
  const mTrader = 1 + TRADER_BONUS/100;
  return mDist * mBarg * mTrader;
}

function render() {
  // keep level within valid range for selected rank
  const r = el.tradeRank.value;
  const lvl = clampLevel(r, el.tradeLevel.value);
  if(String(lvl) !== String(el.tradeLevel.value)) el.tradeLevel.value = String(lvl);

  const bPct = el.applyBargain.checked ? bargainBonusPct(r, lvl) : 0;
  el.bargainOut.innerHTML =
    `Bargain bonus: <span class="mono">${fmt(bPct,2)}%</span>` +
    ` <span class="muted">(rank: ${escapeHtml(r)} ${lvl})</span>`;

  const origin = nodesByName.get(el.originSelect.value);
  if(!origin) {
    el.bestOut.textContent = "-";
    el.bestDetails.textContent = "Pick an origin.";
    el.saleOut.textContent = "-";
    el.saleDetails.textContent = "-";
    el.rowsBody.innerHTML = `<tr><td colspan="4" class="small">Pick an origin.</td></tr>`;
    return;
  }

  const basePrice = Number(el.fishPrice.value || 0);

  const rows = [];
  for(const d of SELL_TARGETS) {
    if(d.name === origin.name) continue;
    const dist = mapDistance(origin, d);
    const distPct = distanceBonusPct(dist);
    const npcs = tmNpcByNode.get(d.name) || [];
    const m = multiplier(distPct, bPct);
    const est = basePrice > 0 ? basePrice * m : NaN;
    rows.push({dest:d.name, npc:npcs.join(", "), distPct, dist, est});
  }
  rows.sort((a,b)=> b.distPct - a.distPct || b.dist - a.dist);

  const best = rows[0];
  const bestPct = best ? best.distPct : 0;
  const ties = rows.filter(r => Math.abs(r.distPct - bestPct) < 0.01).slice(0, 8);

  el.bestOut.textContent = ties.map(t=>t.dest).join(", ");
  el.bestDetails.innerHTML =
    `Origin: <span class="mono">${escapeHtml(origin.name)}</span> &rarr; Distance bonus: <span class="mono">${fmt(bestPct,2)}%</span>`;

  if(basePrice > 0 && best) {
    const m = multiplier(bestPct, bPct);
    el.saleOut.textContent = fmtInt(basePrice * m);
    el.saleDetails.innerHTML =
      `Multiplier: <span class="mono">${fmt(m,4)}&times;</span> &bull; Trader: <span class="mono">+30%</span>` +
      (bPct ? ` &bull; Bargain: <span class="mono">+${fmt(bPct,2)}%</span>` : "");
  } else {
    el.saleOut.textContent = "-";
    el.saleDetails.textContent = "Enter fish price to estimate.";
  }

  const out = [];
  for(const row of rows.slice(0, 50)) {
    out.push(`<tr>
      <td>${escapeHtml(row.dest)}</td>
      <td>${escapeHtml(row.npc)}</td>
      <td class="right mono">${fmt(row.distPct,2)}%</td>
      <td class="right mono">${basePrice > 0 ? fmtInt(row.est) : "-"}</td>
    </tr>`);
  }
  el.rowsBody.innerHTML = out.join("") || `<tr><td colspan="4" class="small">No results.</td></tr>`;
}

let tradeToastTimer=null,tradeSearchTimer=null,tradeRenderFrame=0;
function notifyTradeRoute(){clearTimeout(tradeToastTimer);tradeToastTimer=setTimeout(()=>{const origin=el.originSelect.value;if(origin)NotificationService.ShowInfo(`Route results updated from ${origin}.`,"Trade route recalculated")},420)}
function scheduleTradeRender(){
  if(tradeRenderFrame)return;
  tradeRenderFrame=requestAnimationFrame(()=>{tradeRenderFrame=0;render()});
}
el.originSearch.addEventListener("input", () => {
  clearTimeout(tradeSearchTimer);
  tradeSearchTimer=setTimeout(()=>{refreshOriginMatches();render()},120);
});
el.originSelect.addEventListener("change",()=>{render();if(el.originSelect.value)NotificationService.ShowInfo(`${el.originSelect.value} selected.`,"Origin selected");notifyTradeRoute()});
el.fishPrice.addEventListener("input",()=>{scheduleTradeRender();if(el.fishPrice.value!==""&&Number(el.fishPrice.value)<=0)NotificationService.ShowError("Enter a fish price greater than zero.","Invalid fish price");else notifyTradeRoute()});
el.tradeRank.addEventListener("change",()=>{render();notifyTradeRoute()});
el.tradeLevel.addEventListener("input",()=>{scheduleTradeRender();notifyTradeRoute()});
el.applyBargain.addEventListener("change",()=>{render();notifyTradeRoute()});

refreshOriginMatches();
render();

const marketState = {
  initialized: false,
  items: [],
  selected: null,
  analytics: null,
  outfits: null,
  requestNumber: 0,
  outfitRequestNumber: 0,
  pending: new Map(),
};

const portraitState = {
  initialized:false,
  faceTextureFolder:"",
  oldImage:null,
  newImage:null,
  previewTimer:null,
};

const fontState = {
  initialized:false,
  bdoFolder:"",
  customFont:null,
  presets:[],category:"all",favorites:new Set((()=>{try{return JSON.parse(localStorage.getItem("bdoFontFavorites")||"[]")}catch{return[]}})()),
};

const marketEl = {
  provider: document.getElementById("marketProvider"),
  status: document.getElementById("marketStatus"),
  search: document.getElementById("marketSearch"),
  searchResults: document.getElementById("marketSearchResults"),
  regionButtons: [...document.querySelectorAll("[data-market-region]")],
  export: document.getElementById("marketExport"),
  trackedFilter: document.getElementById("trackedFilter"),
  trackedSort: document.getElementById("trackedSort"),
  trackedCount: document.getElementById("trackedCount"),
  trackedItems: document.getElementById("trackedItems"),
  empty: document.getElementById("marketEmpty"),
  detail: document.getElementById("marketDetail"),
  detailName: document.getElementById("detailName"),
  detailMeta: document.getElementById("detailMeta"),
  range: document.getElementById("historyRange"),
  remove: document.getElementById("removeTracked"),
  current: document.getElementById("metricCurrent"),
  min: document.getElementById("metricMin"),
  max: document.getElementById("metricMax"),
  average: document.getElementById("metricAverage"),
  trend: document.getElementById("metricTrend"),
  salesGrid: document.getElementById("salesGrid"),
  priceChart: document.getElementById("priceChart"),
  salesChart: document.getElementById("salesChart"),
  topOutfitCards: document.getElementById("topOutfitCards"),
  outfitCoverage: document.getElementById("outfitCoverage"),
  outfitFilter: document.getElementById("outfitFilter"),
  outfitRows: document.getElementById("outfitRows"),
};

const BRIDGE_TIMEOUTS={selectGrindLootImage:120000,scanGrindLootImage:120000,downloadAndInstallUpdate:600000,refreshEvents:75000,initializeEvents:75000};
function bridgeCall(command, payload = {}) {
  if(!window.chrome?.webview) return Promise.reject(new Error("The Windows application bridge is unavailable."));
  const id = `market-${++marketState.requestNumber}`;
  return new Promise((resolve, reject) => {
    const timeout=setTimeout(()=>{
      if(!marketState.pending.delete(id))return;
      try{window.chrome.webview.postMessage({id:`cancel-${++marketState.requestNumber}`,command:"cancelRequest",payload:{requestId:id}})}catch{}
      reject(new Error("The operation timed out. Please try again."));
    },BRIDGE_TIMEOUTS[command]||45000);
    marketState.pending.set(id, {resolve, reject, timeout});
    try{window.chrome.webview.postMessage({id, command, payload})}catch(error){clearTimeout(timeout);marketState.pending.delete(id);reject(error)}
  });
}

window.addEventListener("pagehide",()=>{marketState.pending.forEach(pending=>{clearTimeout(pending.timeout);pending.reject(new Error("The application page closed."))});marketState.pending.clear()});

const portraitEl = {
  folderPath:document.getElementById("portraitFolderPath"),
  selectFolder:document.getElementById("portraitSelectFolder"),
  selectOld:document.getElementById("portraitSelectOld"),
  selectNew:document.getElementById("portraitSelectNew"),
  oldPreview:document.getElementById("portraitOldPreview"),
  oldPlaceholder:document.getElementById("portraitOldPlaceholder"),
  oldMeta:document.getElementById("portraitOldMeta"),
  newPreview:document.getElementById("portraitNewPreview"),
  newPlaceholder:document.getElementById("portraitNewPlaceholder"),
  newMeta:document.getElementById("portraitNewMeta"),
  finalPreview:document.getElementById("portraitFinalPreview"),
  finalPlaceholder:document.getElementById("portraitFinalPlaceholder"),
  finalMeta:document.getElementById("portraitFinalMeta"),
  cropModes:[...document.querySelectorAll('input[name="portraitCropMode"]')],
  cropEditor:document.getElementById("portraitCropEditor"),
  cropX:document.getElementById("portraitCropX"),
  cropY:document.getElementById("portraitCropY"),
  zoom:document.getElementById("portraitZoom"),
  zoomOut:document.getElementById("portraitZoomOut"),
  zoomIn:document.getElementById("portraitZoomIn"),
  cropXValue:document.getElementById("portraitCropXValue"),
  cropYValue:document.getElementById("portraitCropYValue"),
  zoomValue:document.getElementById("portraitZoomValue"),
  resetCrop:document.getElementById("portraitResetCrop"),
  status:document.getElementById("portraitStatus"),
  openBackups:document.getElementById("portraitOpenBackups"),
  restore:document.getElementById("portraitRestore"),
  replace:document.getElementById("portraitReplace"),
};

const fontEl = {
  bdoFolder:document.getElementById("fontBdoFolder"),
  selectBdoFolder:document.getElementById("fontSelectBdoFolder"),
  presetGallery:document.getElementById("fontPresetGallery"),
  presetSearch:document.getElementById("fontPresetSearch"),categoryButtons:[...document.querySelectorAll("[data-font-category]")],
  chooseCustom:document.getElementById("fontChooseCustom"),
  customEmpty:document.getElementById("fontCustomEmpty"),
  customLoaded:document.getElementById("fontCustomLoaded"),
  customName:document.getElementById("fontCustomName"),
  customFile:document.getElementById("fontCustomFile"),
  customPreview:document.getElementById("fontCustomPreview"),
  applyCustom:document.getElementById("fontApplyCustom"),
  openFolder:document.getElementById("fontOpenFolder"),
  restoreBackup:document.getElementById("fontRestoreBackup"),
  removeCustom:document.getElementById("fontRemoveCustom"),
  status:document.getElementById("fontStatus"),
};

function setFontStatus(message, kind="") {
  fontEl.status.textContent = message;
  fontEl.status.classList.toggle("negative", kind === "error");
  fontEl.status.classList.toggle("positive", kind === "success");
  if(kind==="error")NotificationService.ShowError(message);if(kind==="success")NotificationService.ShowSuccess(message);
}

function requireBdoFolder() {
  if(!fontState.bdoFolder) {
    setFontStatus("Select the main Black Desert Online folder first.", "error");
    return false;
  }
  return true;
}

function renderFontPresets(presets) {
  fontState.presets=presets||fontState.presets;const query=norm(fontEl.presetSearch?.value),visible=fontState.presets.filter(p=>{const text=norm(`${p.name} ${p.description}`),favorite=fontState.favorites.has(p.id),categoryMatch=fontState.category==="all"||(fontState.category==="favorites"&&favorite)||(fontState.category==="serif"&&/serif/.test(text)&&!/sans/.test(text))||(fontState.category==="sans"&&/sans/.test(text))||(fontState.category==="display"&&/(display|script|decorative|hand)/.test(text));return categoryMatch&&(!query||text.includes(query))});
  if(!visible.length) {
    fontEl.presetGallery.innerHTML = `
      <div class="fontPresetCard">
        No fonts match the selected category or search.
      </div>`;
    return;
  }
  fontEl.presetGallery.innerHTML = visible.map(preset => `
    <article class="fontPresetCard">
      <button class="fontFavorite ${fontState.favorites.has(preset.id)?"active":""}" data-favorite-font="${escapeHtml(preset.id)}" title="Favorite font">&#9733;</button>
      <div class="fontPresetTop">
        <div>
          <strong>${escapeHtml(preset.name)}</strong>
          <span>${escapeHtml(preset.description)}</span>
        </div>
        <button class="marketButton primary" data-apply-font-preset="${escapeHtml(preset.id)}">Apply</button>
      </div>
      <div class="fontPreview">
        <img src="${preset.previewDataUrl}" alt="${escapeHtml(preset.name)} preview" />
      </div>
    </article>
  `).join("");
}
fontEl.presetSearch?.addEventListener("input",()=>renderFontPresets(fontState.presets));fontEl.categoryButtons.forEach(b=>b.addEventListener("click",()=>{fontState.category=b.dataset.fontCategory;fontEl.categoryButtons.forEach(x=>x.classList.toggle("active",x===b));renderFontPresets(fontState.presets)}));

async function initializeFontChanger() {
  if(fontState.initialized) return;
  fontState.initialized = true;
  try {
    const [settings, gallery] = await Promise.all([
      bridgeCall("getFontChangerSettings"),
      bridgeCall("getFontPresets")
    ]);
    fontState.bdoFolder = settings.bdoFolder || "";
    fontEl.bdoFolder.textContent = fontState.bdoFolder || "No BDO folder selected";
    renderFontPresets(gallery.presets || []);
    setFontStatus(fontState.bdoFolder
      ? "BDO folder loaded. Choose a preset or a custom TrueType font."
      : "Select your Black Desert Online folder to begin.");
  } catch(error) {
    setFontStatus(error.message, "error");
  }
}

fontEl.selectBdoFolder.addEventListener("click", async () => {
  try {
    const result = await bridgeCall("selectBdoFolder", {
      currentPath:fontState.bdoFolder
    });
    if(result.cancelled) return;
    fontState.bdoFolder = result.bdoFolder || "";
    fontEl.bdoFolder.textContent = fontState.bdoFolder;
    setFontStatus("BDO folder saved. Choose a font to apply.");
  } catch(error) {
    setFontStatus(error.message, "error");
  }
});

fontEl.presetGallery.addEventListener("click", async event => {
  const favorite=event.target.closest("[data-favorite-font]");if(favorite){const id=favorite.dataset.favoriteFont;fontState.favorites.has(id)?fontState.favorites.delete(id):fontState.favorites.add(id);localStorage.setItem("bdoFontFavorites",JSON.stringify([...fontState.favorites]));renderFontPresets(fontState.presets);return}
  const button = event.target.closest("[data-apply-font-preset]");
  if(!button || !requireBdoFolder()) return;
  try {
    button.disabled = true;
    setFontStatus("Validating and installing the selected font...");
    const result = await bridgeCall("applyPresetFont", {
      bdoFolder:fontState.bdoFolder,
      presetId:button.dataset.applyFontPreset
    });
    setFontStatus(result.message, "success");
    NotificationService.ShowSuccess("Original pearl.ttf backed up.");
    NotificationService.ShowWarning("Restart BDO for font changes to appear.");
  } catch(error) {
    setFontStatus(error.message, "error");
  } finally {
    button.disabled = false;
  }
});

fontEl.chooseCustom.addEventListener("click", async () => {
  try {
    const result = await bridgeCall("selectCustomFont", {
      currentPath:fontState.customFont?.path || ""
    });
    if(result.cancelled) return;
    fontState.customFont = result.font;
    fontEl.customName.textContent = result.font.familyName;
    fontEl.customFile.textContent = result.font.fileName;
    fontEl.customPreview.src = result.font.previewDataUrl;
    fontEl.customEmpty.hidden = true;
    fontEl.customLoaded.hidden = false;
    setFontStatus("Custom font validated. Review the preview, then apply it.");
    NotificationService.ShowInfo(`${result.font.fileName} selected.`,"Custom font ready");
  } catch(error) {
    setFontStatus(error.message, "error");
  }
});

fontEl.applyCustom.addEventListener("click", async () => {
  if(!requireBdoFolder()) return;
  if(!fontState.customFont?.path) {
    setFontStatus("Choose a custom .ttf font first.", "error");
    return;
  }
  try {
    fontEl.applyCustom.disabled = true;
    setFontStatus("Backing up the current font and installing your custom font...");
    const result = await bridgeCall("applyCustomFont", {
      bdoFolder:fontState.bdoFolder,
      fontPath:fontState.customFont.path
    });
    setFontStatus(result.message, "success");
    NotificationService.ShowSuccess("Original pearl.ttf backed up.");
    NotificationService.ShowSuccess("Custom font installed.");
    NotificationService.ShowWarning("Restart BDO for font changes to appear.");
  } catch(error) {
    setFontStatus(error.message, "error");
  } finally {
    fontEl.applyCustom.disabled = false;
  }
});

fontEl.openFolder.addEventListener("click", async () => {
  if(!requireBdoFolder()) return;
  try {
    const result = await bridgeCall("openBdoFontFolder", {
      bdoFolder:fontState.bdoFolder
    });
    setFontStatus(`Font folder opened: ${result.path}`);
  } catch(error) {
    setFontStatus(error.message, "error");
  }
});

fontEl.restoreBackup.addEventListener("click", async () => {
  if(!requireBdoFolder()) return;
  try {
    fontEl.restoreBackup.disabled = true;
    setFontStatus("Restoring the latest pearl.ttf backup...");
    const result = await bridgeCall("restoreLastFontBackup", {
      bdoFolder:fontState.bdoFolder
    });
    setFontStatus(result.message, "success");
  } catch(error) {
    setFontStatus(error.message, "error");
  } finally {
    fontEl.restoreBackup.disabled = false;
  }
});

fontEl.removeCustom.addEventListener("click", async () => {
  if(!requireBdoFolder()) return;
  try {
    fontEl.removeCustom.disabled = true;
    setFontStatus("Backing up and removing pearl.ttf...");
    const result = await bridgeCall("removeCustomFont", {
      bdoFolder:fontState.bdoFolder
    });
    setFontStatus(result.message, result.removed ? "success" : "");
  } catch(error) {
    setFontStatus(error.message, "error");
  } finally {
    fontEl.removeCustom.disabled = false;
  }
});

function portraitCropMode() {
  return portraitEl.cropModes.find(input => input.checked)?.value || "crop";
}

function portraitCropValueLabel(value, low, high) {
  const number = Number(value);
  if(number <= 5) return low;
  if(number >= 95) return high;
  if(Math.abs(number - 50) <= 4) return "Center";
  return `${number}%`;
}

function updatePortraitCropLabels() {
  portraitEl.cropXValue.textContent =
    portraitCropValueLabel(portraitEl.cropX.value, "Far left", "Far right");
  portraitEl.cropYValue.textContent =
    portraitCropValueLabel(portraitEl.cropY.value, "Top", "Bottom");
  portraitEl.zoomValue.textContent = `${portraitEl.zoom.value}%`;
  const awaitingImage = !portraitState.newImage?.path;
  const disabled = awaitingImage || portraitCropMode() === "stretch";
  portraitEl.cropEditor.classList.toggle("disabled", disabled);
  portraitEl.cropEditor.classList.toggle("awaiting-image", awaitingImage);
  portraitEl.cropModes.forEach(input => input.disabled = awaitingImage);
  portraitEl.cropX.disabled = disabled;
  portraitEl.cropY.disabled = disabled;
  portraitEl.zoom.disabled = disabled;
  portraitEl.zoomOut.disabled = disabled;
  portraitEl.zoomIn.disabled = disabled;
  portraitEl.resetCrop.disabled = disabled;
}

function setPortraitStatus(message, kind="") {
  portraitEl.status.textContent = message;
  portraitEl.status.classList.toggle("negative", kind === "error");
  portraitEl.status.classList.toggle("positive", kind === "success");
  if(kind==="error")NotificationService.ShowError(message);if(kind==="success")NotificationService.ShowSuccess(message);
}

function showPortraitImage(imageElement, placeholderElement, dataUrl) {
  const hasImage = Boolean(dataUrl);
  imageElement.hidden = !hasImage;
  placeholderElement.hidden = hasImage;
  if(!hasImage) {
    imageElement.removeAttribute("src");
    return;
  }
  imageElement.onerror = () => {
    imageElement.hidden = true;
    imageElement.removeAttribute("src");
    placeholderElement.hidden = false;
  };
  imageElement.src = dataUrl;
}

function clearPortraitFinalPreview() {
  showPortraitImage(portraitEl.finalPreview, portraitEl.finalPlaceholder, "");
  portraitEl.finalMeta.textContent = portraitCropMode() === "stretch"
    ? "Stretch fills 624 &times; 804 but may distort the image."
    : "Move the crop sliders below to choose the exact framing.";
}

function schedulePortraitPreview() {
  clearTimeout(portraitState.previewTimer);
  updatePortraitCropLabels();
  if(!portraitState.newImage?.path) {
    clearPortraitFinalPreview();
    return;
  }
  portraitState.previewTimer = setTimeout(refreshPortraitPreview, 90);
}

async function refreshPortraitPreview() {
  if(!portraitState.newImage?.path) return;
  try {
    const result = await bridgeCall("previewPortrait", {
      newImagePath:portraitState.newImage.path,
      cropMode:portraitCropMode(),
      cropX:Number(portraitEl.cropX.value),
      cropY:Number(portraitEl.cropY.value),
      zoom:Number(portraitEl.zoom.value) / 100,
    });
    showPortraitImage(
      portraitEl.finalPreview,
      portraitEl.finalPlaceholder,
      result.previewDataUrl);
    portraitEl.finalMeta.textContent =
      `Final output: 624 &times; 804 BMP &bull; ${portraitCropMode() === "stretch" ? "Stretched" : `Manual crop &bull; ${portraitEl.zoom.value}% zoom`}`;
  } catch(error) {
    clearPortraitFinalPreview();
    setPortraitStatus(error.message, "error");
  }
}

async function initializePortraitReplacer() {
  if(portraitState.initialized) return;
  portraitState.initialized = true;
  updatePortraitCropLabels();
  try {
    const settings = await bridgeCall("getPortraitSettings");
    portraitState.faceTextureFolder = settings.faceTextureFolder || "";
    portraitEl.folderPath.textContent =
      portraitState.faceTextureFolder || "No folder selected";
    setPortraitStatus(portraitState.faceTextureFolder
      ? "FaceTexture folder loaded. Select the portrait you want to replace."
      : "Select your FaceTexture folder to begin.");
  } catch(error) {
    setPortraitStatus(error.message, "error");
  }
}

portraitEl.selectFolder.addEventListener("click", async () => {
  try {
    const result = await bridgeCall("selectFaceTextureFolder", {
      currentPath:portraitState.faceTextureFolder
    });
    if(result.cancelled) return;
    portraitState.faceTextureFolder = result.faceTextureFolder || "";
    portraitState.oldImage = null;
    portraitEl.folderPath.textContent = portraitState.faceTextureFolder;
    showPortraitImage(portraitEl.oldPreview, portraitEl.oldPlaceholder, "");
    portraitEl.oldMeta.textContent = "No old portrait selected";
    setPortraitStatus("FaceTexture folder saved. Select the existing .bmp portrait.");
  } catch(error) {
    setPortraitStatus(error.message, "error");
  }
});

portraitEl.selectOld.addEventListener("click", async () => {
  try {
    const result = await bridgeCall("selectOldPortrait", {
      faceTextureFolder:portraitState.faceTextureFolder
    });
    if(result.cancelled) return;
    portraitState.oldImage = result.image;
    showPortraitImage(
      portraitEl.oldPreview,
      portraitEl.oldPlaceholder,
      result.image.previewDataUrl);
    portraitEl.oldMeta.textContent =
      `${result.image.fileName} &bull; ${result.image.width} &times; ${result.image.height}`;
    setPortraitStatus(`Ready to replace ${result.image.fileName}. Select the new image.`);
    NotificationService.ShowInfo(`${result.image.fileName} selected.`,"Existing portrait selected");
  } catch(error) {
    setPortraitStatus(error.message, "error");
  }
});

portraitEl.selectNew.addEventListener("click", async () => {
  try {
    const result = await bridgeCall("selectNewPortrait", {
      currentPath:portraitState.newImage?.path || ""
    });
    if(result.cancelled) return;
    portraitState.newImage = result.image;
    showPortraitImage(
      portraitEl.newPreview,
      portraitEl.newPlaceholder,
      result.image.previewDataUrl);
    portraitEl.newMeta.textContent =
      `${result.image.fileName} &bull; ${result.image.width} &times; ${result.image.height}`;
    setPortraitStatus("New image loaded. Adjust the crop, then review the final preview.");
    NotificationService.ShowInfo(`${result.image.fileName} selected.`,"New image selected");
    updatePortraitCropLabels();
    schedulePortraitPreview();
  } catch(error) {
    setPortraitStatus(error.message, "error");
  }
});

portraitEl.cropModes.forEach(input =>
  input.addEventListener("change", schedulePortraitPreview));
portraitEl.cropX.addEventListener("input", schedulePortraitPreview);
portraitEl.cropY.addEventListener("input", schedulePortraitPreview);
portraitEl.zoom.addEventListener("input", schedulePortraitPreview);
portraitEl.zoomOut.addEventListener("click", () => {
  portraitEl.zoom.value = Math.max(100, Number(portraitEl.zoom.value) - 10);
  schedulePortraitPreview();
});
portraitEl.zoomIn.addEventListener("click", () => {
  portraitEl.zoom.value = Math.min(300, Number(portraitEl.zoom.value) + 10);
  schedulePortraitPreview();
});
portraitEl.resetCrop.addEventListener("click", () => {
  portraitEl.cropX.value = "50";
  portraitEl.cropY.value = "50";
  portraitEl.zoom.value = "100";
  schedulePortraitPreview();
});

portraitEl.replace.addEventListener("click", async () => {
  if(!portraitState.faceTextureFolder || !portraitState.oldImage?.path || !portraitState.newImage?.path) {
    setPortraitStatus("Select the FaceTexture folder, old portrait, and new image first.", "error");
    return;
  }
  try {
    portraitEl.replace.disabled = true;
    setPortraitStatus("Converting the image and creating a safety backup...");
    const result = await bridgeCall("replacePortrait", {
      faceTextureFolder:portraitState.faceTextureFolder,
      oldImagePath:portraitState.oldImage.path,
      newImagePath:portraitState.newImage.path,
      cropMode:portraitCropMode(),
      cropX:Number(portraitEl.cropX.value),
      cropY:Number(portraitEl.cropY.value),
      zoom:Number(portraitEl.zoom.value) / 100,
    });
    portraitState.oldImage.previewDataUrl = result.previewDataUrl;
    showPortraitImage(
      portraitEl.oldPreview,
      portraitEl.oldPlaceholder,
      result.previewDataUrl);
    setPortraitStatus(
      `${result.fileName} was backed up and replaced successfully. Restart BDO to see the new portrait.`,
      "success");
    NotificationService.ShowSuccess("Backup saved successfully.");NotificationService.ShowSuccess("Portrait converted to 624 &times; 804 BMP.");NotificationService.ShowSuccess("Replacement complete.");NotificationService.ShowWarning("Restart BDO for portrait changes to appear.");
  } catch(error) {
    setPortraitStatus(error.message, "error");
  } finally {
    portraitEl.replace.disabled = false;
  }
});

portraitEl.openBackups.addEventListener("click", async () => {
  try {
    const result = await bridgeCall("openPortraitBackupFolder", {
      faceTextureFolder:portraitState.faceTextureFolder
    });
    setPortraitStatus(`Backup folder opened: ${result.backupFolder}`);
  } catch(error) {
    setPortraitStatus(error.message, "error");
  }
});

portraitEl.restore.addEventListener("click", async () => {
  if(!portraitState.oldImage?.path) {
    setPortraitStatus("Select the portrait file you want to restore first.", "error");
    return;
  }
  try {
    portraitEl.restore.disabled = true;
    setPortraitStatus("Restoring the most recent backup...");
    const result = await bridgeCall("restoreLastPortraitBackup", {
      faceTextureFolder:portraitState.faceTextureFolder,
      oldImagePath:portraitState.oldImage.path
    });
    showPortraitImage(
      portraitEl.oldPreview,
      portraitEl.oldPlaceholder,
      result.previewDataUrl);
    setPortraitStatus(
      `${result.fileName} was restored from the latest backup.`,
      "success");
  } catch(error) {
    setPortraitStatus(error.message, "error");
  } finally {
    portraitEl.restore.disabled = false;
  }
});

window.chrome?.webview?.addEventListener("message", event => {
  const message = event.data || {};
  if(message.id && marketState.pending.has(message.id)) {
    const pending = marketState.pending.get(message.id);
    marketState.pending.delete(message.id);
    clearTimeout(pending.timeout);
    if(message.ok) pending.resolve(message.data);
    else pending.reject(new Error(message.error || "The operation failed."));
    return;
  }
  if(message.eventName === "status" && message.data?.message) {
    setMarketStatus(message.data.message);
  }
  if(message.eventName === "dataChanged" && marketState.initialized) {
    refreshMarketState();
  }
  if(message.eventName === "updateCheckRequested") {
    initializeUpdateChecker();
  }
});

function setMarketStatus(message, isError = false) {
  marketEl.status.textContent = message;
  marketEl.status.classList.toggle("negative", isError);
}

function getMarketRegion() {
  return "eu";
}

function setMarketRegion() {
  const normalized = "eu";
  marketEl.regionButtons.forEach(button => {
    const active = button.dataset.marketRegion === normalized;
    button.classList.toggle("active", active);
    button.setAttribute("aria-selected", active ? "true" : "false");
  });
}

function marketRegionPanels() {
  return [document.getElementById("trackerPanel"), document.getElementById("outfitPanel")].filter(Boolean);
}

function waitForMarketFade() {
  return new Promise(resolve => setTimeout(resolve, 140));
}

async function initializeMarket() {
  if(marketState.initialized) return;
  try {
    setMarketStatus("Loading local market history...");
    const state = await bridgeCall("initialize");
    marketState.initialized = true;
    marketEl.provider.textContent = `${state.provider} + local SQLite history`;
    setMarketRegion(state.settings?.region || "eu");
    await loadMarketRegionState(getMarketRegion(), false);
    renderTrackedItems();
    setMarketStatus("Ready");
  } catch(error) {
    setMarketStatus(error.message, true);
  }
}

async function refreshMarketState() {
  try {
    await loadMarketRegionState(getMarketRegion(), false);
    renderTrackedItems();
    if(marketState.selected) {
      const stillTracked = marketState.items.find(item =>
        item.itemId === marketState.selected.itemId && item.enhancement === marketState.selected.enhancement);
      if(stillTracked) {
        marketState.selected = stillTracked;
        await loadAnalytics();
      } else {
        clearMarketDetail();
      }
    }
  } catch(error) {
    setMarketStatus(error.message, true);
  }
}

const couponState={initialized:false,coupons:[],activeTab:"available",selectedCode:"",page:0,pageSize:8,timer:null,autoTimer:null,autoRefreshing:false,lastKnownCodes:new Set(readSetting("couponKnownCodes",[]))};
const couponEl={search:document.getElementById("couponSearch"),status:document.getElementById("couponStatusFilter"),showExpired:document.getElementById("couponShowExpired"),refresh:document.getElementById("couponRefresh"),source:document.getElementById("couponSourceBadge"),updated:document.getElementById("couponLastUpdated"),message:document.getElementById("couponMessage"),rows:document.getElementById("couponRows"),available:document.getElementById("couponAvailableCount"),availableTab:document.getElementById("couponAvailableTabCount"),expired:document.getElementById("couponExpiredCount"),expiredTab:document.getElementById("couponExpiredTabCount"),redeemed:document.getElementById("couponRedeemedCount"),total:document.getElementById("couponTotalCount"),sort:document.getElementById("couponSort"),detail:document.getElementById("couponDetail"),lastCheck:document.getElementById("couponLastCheck"),sync:document.getElementById("couponSyncText"),statusAlert:document.getElementById("couponStatusAlert"),pagePrevious:document.getElementById("couponPagePrevious"),pageNext:document.getElementById("couponPageNext"),pageStatus:document.getElementById("couponPageStatus")};
const updateState={info:null,installing:false};
const updateEl={alert:document.getElementById("updateStatusAlert")};
const appVersionEl=document.getElementById("appVersionLabel");
function applyAppVersion(value){if(!appVersionEl)return;const version=String(value||"").trim();appVersionEl.textContent=version||"v...";appVersionEl.title=version?`BDO Multi-Tool ${version}`:"Application version";}
async function initializeAppVersion(){try{const info=await bridgeCall("getAppVersion");applyAppVersion(info?.version);}catch(error){console.warn("[App] version unavailable",error);}}
function applyUpdateStatus(info){updateState.info=info||null;updateState.installing=false;const show=Boolean(info?.updateAvailable);if(!updateEl.alert)return;updateEl.alert.classList.remove("busy");updateEl.alert.textContent=show?`Update now ${info.latestVersion||""}`:"";updateEl.alert.title=show?"Download, verify, and launch the latest BDO Multi-Tool installer":"";updateEl.alert.classList.toggle("show",show);}
async function installUpdateFromAlert(){const info=updateState.info;if(!info?.updateAvailable||updateState.installing)return;updateState.installing=true;const previous=updateEl.alert?.textContent||"";if(updateEl.alert){updateEl.alert.textContent="Downloading & verifying...";updateEl.alert.classList.add("busy");}try{const result=await bridgeCall("downloadAndInstallUpdate",{latestVersion:info.latestVersion||""});NotificationService.ShowSuccess(`Starting installer ${result.latestVersion||info.latestVersion||""}.`,"Update ready");if(updateEl.alert)updateEl.alert.textContent="Launching installer...";}catch(error){if(updateEl.alert){updateEl.alert.textContent=previous;updateEl.alert.classList.remove("busy");}NotificationService.ShowError(error.message||"Could not download the update installer.");try{await bridgeCall("openExternalUrl",{url:info.url||info.repositoryUrl});}catch{}updateState.installing=false;}}
async function initializeUpdateChecker(){try{const info=await bridgeCall("checkForUpdates");applyUpdateStatus(info);}catch(error){console.warn("[Updates] check failed",error);applyUpdateStatus(null);}}
updateEl.alert?.addEventListener("click",installUpdateFromAlert);
updateEl.alert?.addEventListener("keydown",event=>{if(event.key==="Enter"||event.key===" "){event.preventDefault();installUpdateFromAlert();}});
function couponEscape(value){return String(value??"").replace(/[&<>"']/g,c=>({"&":"&amp;","<":"&lt;",">":"&gt;",'"':"&quot;","'":"&#39;"}[c]));}
function couponRedeemedMap(){return readSetting("couponRedeemed",{})}
function couponIsRedeemed(code){return couponRedeemedMap()[String(code||"").toUpperCase()]===true}
function couponUnreadNewCodes(){return readSetting("couponNewCodes",[]).map(code=>String(code||"").toUpperCase()).filter(Boolean)}
function couponNewMessage(count){return count?`${count} new coupon${count===1?" is":"s are"} available`:""}
function couponSetTaskbarBadge(count){bridgeCall("setCouponBadgeCount",{count:Math.max(0,Number(count)||0)}).catch(()=>{});}
function couponSetUnreadNewCodes(codes){const unique=[...new Set((codes||[]).map(code=>String(code||"").toUpperCase()).filter(Boolean))],active=new Set(couponState.coupons.filter(c=>!c.isExpired).map(c=>String(c.code||"").toUpperCase()));const unread=unique.filter(code=>active.has(code)&&!couponIsRedeemed(code));persistSetting("couponNewCodes",unread);couponSetStatusAlert(couponNewMessage(unread.length));couponSetTaskbarBadge(unread.length);return unread.length}
function couponPruneNewCouponAlert(){return couponSetUnreadNewCodes(couponUnreadNewCodes())}
function couponClearNewCode(code){const key=String(code||"").toUpperCase();if(!key)return;couponSetUnreadNewCodes(couponUnreadNewCodes().filter(x=>x!==key))}
function setCouponRedeemed(code,redeemed){const map=couponRedeemedMap(),key=String(code||"").toUpperCase();if(!key)return;if(redeemed){map[key]=true;couponClearNewCode(key)}else delete map[key];persistSetting("couponRedeemed",map)}
function couponRedeemButton(c){const redeemed=couponIsRedeemed(c.code);return `<button class="couponRedeemToggle ${redeemed?"redeemed":""}" data-redeem-coupon="${couponEscape(c.code)}" aria-pressed="${redeemed}" title="Mark this coupon as ${redeemed?"not redeemed":"redeemed"}">${redeemed?"Redeemed":"Redeem"}</button>`}
function couponVisibleAvailable(){return couponState.coupons.filter(c=>!c.isExpired&&!couponIsRedeemed(c.code))}
function couponVisibleRedeemed(){return couponState.coupons.filter(c=>couponIsRedeemed(c.code))}
function couponSetStatusAlert(message){if(!couponEl.statusAlert)return;couponEl.statusAlert.textContent=message||"";couponEl.statusAlert.classList.toggle("show",Boolean(message));}
function couponRememberCodes(coupons){const codes=(coupons||[]).map(c=>String(c.code||"").toUpperCase()).filter(Boolean);couponState.lastKnownCodes=new Set(codes);persistSetting("couponKnownCodes",codes)}
function couponNotifyNewCodes(coupons,{silent=false}={}){const active=(coupons||[]).filter(c=>!c.isExpired).map(c=>String(c.code||"").toUpperCase()).filter(Boolean);if(!couponState.lastKnownCodes.size){couponRememberCodes(coupons);couponPruneNewCouponAlert();return 0;}const fresh=active.filter(code=>!couponState.lastKnownCodes.has(code)&&!couponIsRedeemed(code));couponRememberCodes(coupons);if(!fresh.length){couponPruneNewCouponAlert();return 0;}const count=couponSetUnreadNewCodes([...couponUnreadNewCodes(),...fresh]);const message=couponNewMessage(count);if(!silent&&message)NotificationService.ShowSuccess(message,"Coupons");return fresh.length}
function couponRows(){const search=couponEl.search.value.trim().toLowerCase(),status=couponEl.status.value;return couponState.coupons.filter(c=>{const redeemed=couponIsRedeemed(c.code);if(couponState.activeTab==="available"&&(c.isExpired||redeemed))return false;if(couponState.activeTab==="redeemed"&&!redeemed)return false;if(couponState.activeTab==="expired"&&!c.isExpired)return false;if(status==="available"&&c.isExpired)return false;if(status==="expired"&&!c.isExpired)return false;return !search||`${c.code} ${(c.rewards||[]).map(r=>r.itemName).join(" ")}`.toLowerCase().includes(search);});}
function couponRewards(rewards){const list=Array.isArray(rewards)?rewards:[],shown=list.slice(0,7),compact=list.length>2;if(!list.length)return'<span class="couponRewardMore">Reward details unavailable</span>';return`<div class="couponRewards">${shown.map((r,i)=>`<span class="couponReward" title="${couponEscape(`${r.quantity}x ${r.itemName}`)}"><img class="couponRewardIcon" src="${couponEscape(r.icon)}" alt="">${!compact||i===0?`<span class="couponRewardText">${couponEscape(r.quantity)}x ${couponEscape(r.itemName)}</span>`:""}</span>`).join("")}${compact?`<span class="couponRewardMore">+${Math.max(0,list.length-1)} items</span>`:""}</div>`;}
function couponExpiryText(value,expired,fallback){const date=new Date(value);if(!value||Number.isNaN(date.getTime()))return fallback||"No expiry listed";const diff=date.getTime()-Date.now();const days=Math.ceil(Math.abs(diff)/86400000);if(diff<=0)return`Expired ${Math.max(1,days)} day${days===1?"":"s"} ago`;return diff<86400000?"Expires today":`${Math.max(1,days)} days`;}
function couponCacheAgeText(minutes){const value=Number(minutes);if(!Number.isFinite(value)||value<=0)return"";if(value<60)return`${Math.max(1,Math.round(value))}m`;const hours=Math.round(value/60);if(hours<48)return`${hours}h`;return`${Math.round(hours/24)}d`}
function applyCouponDashboard(data,{checkNew=false,silent=false}={}){couponState.coupons=Array.isArray(data.coupons)?data.coupons:[];const settings=data.settings||{};couponEl.search.value="";couponEl.status.value=settings.status||"all";couponEl.showExpired.checked=settings.showExpired!==false;const available=couponVisibleAvailable(),redeemed=couponVisibleRedeemed();couponEl.available.textContent=available.length;if(couponEl.availableTab)couponEl.availableTab.textContent=available.length;couponEl.expired.textContent=data.expiredCount??couponState.coupons.filter(x=>x.isExpired).length;if(couponEl.redeemed)couponEl.redeemed.textContent=redeemed.length;couponEl.total.textContent=data.totalCount??couponState.coupons.length;const status=String(data.status||"CACHED").toUpperCase(),cacheAge=couponCacheAgeText(data.cacheAgeMinutes);couponEl.source.textContent=status==="LIVE"?"LIVE DATA":status==="ERROR"?"ERROR":"CACHED DATA";couponEl.sync.textContent=status==="LIVE"?"Synced":data.isStale?`Stored locally${cacheAge?` - ${cacheAge} old`:""}`:"Stored locally";const date=new Date(data.lastAttempt||data.lastRefreshed);couponEl.lastCheck.textContent=Number.isNaN(date.getTime())?"-":date.toLocaleTimeString([],{hour:"2-digit",minute:"2-digit",second:"2-digit"});couponEl.updated.textContent=`Last Attempt: ${Number.isNaN(date.getTime())?"-":date.toLocaleString()}`;couponEl.message.textContent=data.message||((status!=="LIVE"&&data.isStale)?"Showing stored coupon data from the last successful refresh.":"");if(checkNew)couponNotifyNewCodes(couponState.coupons,{silent});else{if(!couponState.lastKnownCodes.size)couponRememberCodes(couponState.coupons);couponPruneNewCouponAlert();}if(!couponState.selectedCode||!couponState.coupons.some(c=>c.code===couponState.selectedCode))couponState.selectedCode=(couponState.activeTab==="redeemed"?redeemed[0]?.code:available[0]?.code)||couponState.coupons[0]?.code||"";renderCoupons();}
function renderCoupons(){
  const rows=couponRows(),sort=couponEl.sort?.value||"newest";
  if(sort==="code")rows.sort((a,b)=>a.code.localeCompare(b.code));
  else if(sort==="oldest")rows.reverse();
  const available=couponVisibleAvailable(),redeemed=couponVisibleRedeemed(),expired=couponState.coupons.filter(c=>c.isExpired);
  couponEl.available.textContent=available.length;
  if(couponEl.availableTab)couponEl.availableTab.textContent=available.length;
  if(couponEl.redeemed)couponEl.redeemed.textContent=redeemed.length;
  if(couponEl.expiredTab)couponEl.expiredTab.textContent=expired.length;

  const pageCount=Math.max(1,Math.ceil(rows.length/couponState.pageSize));
  couponState.page=Math.min(Math.max(0,couponState.page),pageCount-1);
  const pageRows=rows.slice(couponState.page*couponState.pageSize,(couponState.page+1)*couponState.pageSize);
  if(couponEl.pageStatus)couponEl.pageStatus.textContent=`Page ${couponState.page+1} of ${pageCount}`;
  if(couponEl.pagePrevious)couponEl.pagePrevious.disabled=couponState.page===0;
  if(couponEl.pageNext)couponEl.pageNext.disabled=couponState.page>=pageCount-1;
  if(!rows.length){
    couponEl.rows.innerHTML='<div class="couponDetailEmpty">No coupons match the current filters.</div>';
    couponEl.detail.innerHTML='<div class="couponDetailEmpty">No coupon details are available.</div>';
    return;
  }
  if(!couponState.selectedCode||!pageRows.some(c=>c.code===couponState.selectedCode))couponState.selectedCode=pageRows[0].code;
  couponEl.rows.innerHTML=pageRows.map(c=>{
    const rewards=c.rewards||[],first=rewards[0];
    return`<article class="couponRowV2 ${c.code===couponState.selectedCode?"selected":""}" data-coupon-code="${couponEscape(c.code)}"><div class="couponRowCode"><span class="couponTicket">&#9670;</span><span>${couponEscape(c.code)}</span></div><div class="couponRedeemCell">${couponRedeemButton(c)}</div><div class="couponStatusV2"><span>${c.isExpired?"Expired":"Available"}</span></div><div class="couponRewardSummary">${first?`<img src="${couponEscape(first.icon)}" alt=""><span>${couponEscape(first.quantity)}x ${couponEscape(first.itemName)}${rewards.length>1?`<br>+${rewards.length-1} items`:""}</span>`:"Rewards unavailable"}</div><div class="couponChevron">&rsaquo;</div></article>`;
  }).join("");
  renderCouponDetail(pageRows.find(c=>c.code===couponState.selectedCode)||pageRows[0]);
}
function renderCouponDetail(c){const rewards=c.rewards||[],first=rewards[0],redeemed=couponIsRedeemed(c.code);couponEl.detail.innerHTML=`<button class="couponDetailClose">&times;</button><span class="couponDetailBadge">${c.isExpired?"EXPIRED":"AVAILABLE"} <i></i></span><h2>${couponEscape(c.code)} <button class="couponInlineCopy" data-copy-coupon="${couponEscape(c.code)}">&#9633;</button></h2><div class="couponDetailLead">Redeem this code in-game to claim your rewards.</div><div class="couponDetailStats"><div class="couponDetailStat"><span class="couponDetailStatIcon">${redeemed?"&#10003;":"&#9633;"}</span><div><label>REDEMPTION STATUS</label><strong class="${redeemed?"green":"muted"}">${redeemed?"Redeemed":"Not redeemed"}</strong><small>${redeemed?"You marked this coupon as used.":"Mark it redeemed after using it in-game."}</small></div></div><div class="couponDetailStat"><span class="couponDetailStatIcon">&#10003;</span><div><label>COUPON STATUS</label><strong class="green">${c.isExpired?"Expired":"Available"}</strong><small>${couponEscape(couponExpiryText(c.expiryUtc,c.isExpired,c.expiryText))}</small></div></div></div><section class="couponRewardPreview"><h3>REWARDS PREVIEW</h3><div class="couponRewardDetailRow">${first?`<img src="${couponEscape(first.icon)}" alt=""><span>${couponEscape(first.quantity)}x ${couponEscape(first.itemName)}${rewards.length>1?` +${rewards.length-1} items`:""}</span><span class="couponRewardCount">${rewards.length} items</span>`:"Reward details unavailable."}</div></section><button class="couponCopyLarge" data-copy-coupon="${couponEscape(c.code)}">Copy Code</button><button class="couponRedeemOnline" data-open-url="https://payment.naeu.playblackdesert.com/en-us/Shop/Coupon/">Redeem Online &nbsp; &nearr;</button>`;}
async function initializeCoupons(){if(couponState.initialized)return;couponState.initialized=true;try{applyCouponDashboard(await bridgeCall("initializeCoupons"));startCouponAutoRefresh();setTimeout(()=>refreshCoupons({auto:true,silent:false}),1800);}catch(error){couponEl.source.textContent="ERROR";couponEl.source.className="couponSourceBadge error";couponEl.message.textContent=error.message;}}
clearInterval(window.__bdoCouponRelativeTimer);window.__bdoCouponRelativeTimer=setInterval(()=>{if(couponState.initialized&&document.getElementById("couponsView")?.classList.contains("active"))renderCoupons();},60000);
function startCouponAutoRefresh(){clearInterval(couponState.autoTimer);couponState.autoTimer=setInterval(()=>refreshCoupons({auto:true,silent:false}),21600000);}
async function refreshCoupons(options={}){const auto=options.auto===true,silent=options.silent===true;if(couponState.autoRefreshing)return;if(!auto&&couponEl.refresh.disabled)return;const attemptStarted=new Date();console.info("[Coupons] refresh started");couponState.autoRefreshing=true;if(!auto){couponEl.refresh.disabled=true;couponEl.refresh.textContent="Refreshing...";}couponEl.updated.textContent=`Last Attempt: ${attemptStarted.toLocaleString()}`;couponEl.message.textContent=auto?"Auto-checking live coupon sources...":"Trying live coupon source...";try{const data=await bridgeCall("refreshCoupons");const debug=data.refreshDebug||{};console.info("[Coupons] source URL:",debug.sourceUrl||data.sourceUrl||"unknown");console.info("[Coupons] HTTP status:",debug.httpStatus??"unavailable");if((debug.rawResponseLength??0)>0)console.info("[Coupons] raw response length:",debug.rawResponseLength);console.info("[Coupons] parsing succeeded:",debug.parsingSucceeded===true);console.info("[Coupons] coupons parsed:",debug.couponsParsed??0);console.info("[Coupons] cache updated:",debug.cacheUpdated===true?"yes":"no");applyCouponDashboard(data,{checkNew:true,silent});console.info("[Coupons] UI updated: yes");if(!auto&&String(data.status).toUpperCase()==="LIVE"&&debug.parsingSucceeded&&debug.cacheUpdated)NotificationService.ShowSuccess("Coupons refreshed successfully.");else if(!auto){const reason=data.message||debug.failureReason||"Could not refresh coupons. Showing cached data.";console.warn("[Coupons] refresh failed reason:",reason);NotificationService.ShowWarning(reason);}}catch(error){console.error("[Coupons] cache updated: no");console.error("[Coupons] UI updated: no");console.error("[Coupons] refresh failed reason:",error.message);couponEl.source.textContent="ERROR";couponEl.source.className="couponSourceBadge error";couponEl.message.textContent=error.message;if(!auto)NotificationService.ShowError(error.message);}finally{couponState.autoRefreshing=false;if(!auto){couponEl.refresh.disabled=false;couponEl.refresh.textContent="Refresh Coupons";}}}
function saveCouponSettings(){clearTimeout(couponState.timer);couponState.timer=setTimeout(()=>bridgeCall("saveCouponSettings",{showAvailableOnly:couponState.activeTab==="available",showExpired:couponEl.showExpired.checked,search:"",status:couponEl.status.value}).catch(()=>{}),300);}
couponEl.sort?.addEventListener("change",()=>{couponState.page=0;renderCoupons();});
couponEl.rows?.addEventListener("click",event=>{const redeem=event.target.closest("[data-redeem-coupon]");if(redeem){const code=redeem.dataset.redeemCoupon;couponState.selectedCode=code;const next=!couponIsRedeemed(code);setCouponRedeemed(code,next);renderCoupons();NotificationService.ShowSuccess(`${code} marked as ${next?"redeemed":"not redeemed"}.`);return;}if(event.target.closest("[data-copy-coupon]"))return;const row=event.target.closest("[data-coupon-code]");if(row){couponState.selectedCode=row.dataset.couponCode;renderCoupons();}});
couponEl.detail?.addEventListener("click",async event=>{if(event.target.closest(".couponDetailClose")){couponEl.detail.innerHTML='<div class="couponDetailEmpty">Select a coupon to see its details.</div>';return;}const external=event.target.closest("[data-open-url]");if(external){try{await bridgeCall("openExternalUrl",{url:external.dataset.openUrl});}catch(error){NotificationService.ShowError(error.message||"Could not open redeem page.");}return;}const button=event.target.closest("[data-copy-coupon]");if(!button)return;const code=button.dataset.copyCoupon;try{await navigator.clipboard.writeText(code);}catch{}NotificationService.ShowSuccess(`Copied ${code}`);});
couponEl.search?.addEventListener("input",()=>{couponState.page=0;renderCoupons();saveCouponSettings();});
couponEl.status?.addEventListener("change",()=>{couponState.page=0;renderCoupons();saveCouponSettings();});
couponEl.showExpired?.addEventListener("change",()=>{if(!couponEl.showExpired.checked&&couponState.activeTab==="expired"){couponState.activeTab="available";document.querySelectorAll("[data-coupon-tab]").forEach(b=>b.classList.toggle("active",b.dataset.couponTab==="available"));}couponState.page=0;renderCoupons();saveCouponSettings();});
couponEl.pagePrevious?.addEventListener("click",()=>{couponState.page=Math.max(0,couponState.page-1);renderCoupons();});
couponEl.pageNext?.addEventListener("click",()=>{couponState.page+=1;renderCoupons();});
couponEl.refresh?.addEventListener("click",()=>refreshCoupons());
document.querySelectorAll("[data-coupon-tab]").forEach(button=>button.addEventListener("click",()=>{couponState.activeTab=button.dataset.couponTab||"available";couponState.page=0;document.querySelectorAll("[data-coupon-tab]").forEach(x=>x.classList.toggle("active",x===button));renderCoupons();saveCouponSettings();}));
couponEl.rows?.addEventListener("click",async event=>{const button=event.target.closest("[data-copy-coupon]");if(!button)return;const code=button.dataset.copyCoupon;try{await navigator.clipboard.writeText(code);}catch{const input=document.createElement("textarea");input.value=code;document.body.appendChild(input);input.select();document.execCommand("copy");input.remove();}NotificationService.ShowSuccess(`Copied ${code}`);});

const AP_BRACKETS=[[100,139,5],[140,169,10],[170,183,15],[184,208,20],[209,234,30],[235,244,40],[245,248,48],[249,252,57],[253,256,69],[257,260,83],[261,264,101],[265,268,122],[269,272,137],[273,276,142],[277,280,148],[281,284,154],[285,288,160],[289,292,167],[293,296,174],[297,300,181],[301,304,188],[305,308,196],[309,315,200],[316,320,203],[321,327,205],[328,331,208],[332,336,211],[337,341,214],[342,346,217],[347,351,220],[352,357,223],[358,363,225],[364,368,227],[369,374,230],[375,380,233],[381,385,236],[386,391,239],[392,396,242],[397,499,245]],DP_BRACKETS=[[203,210,1],[211,217,2],[218,225,3],[226,232,4],[233,240,5],[241,247,6],[248,255,7],[256,262,8],[263,270,9],[271,277,10],[278,285,11],[286,292,12],[293,300,13],[301,307,14],[308,314,15],[315,321,16],[322,328,17],[329,334,18],[335,340,19],[341,346,20],[347,352,21],[353,358,22],[359,364,23],[365,370,24],[371,376,25],[377,382,26],[383,388,27],[389,394,28],[395,400,29],[401,999,30]],DR_BRACKETS=[[253,255,2],[256,258,4],[259,261,6],[262,264,8],[265,269,10],[270,274,12],[275,278,14],[279,282,16],[283,286,18],[287,289,20],[290,292,22],[293,295,24],[296,298,26],[299,301,28],[302,304,31],[305,307,35],[308,310,37],[311,313,40],[314,316,43],[317,320,46],[321,323,50],[324,325,51],[326,327,52],[328,329,53],[330,331,54],[332,333,55],[334,335,56],[336,337,57],[338,339,58],[340,341,59],[342,344,60],[345,347,61],[348,350,63],[351,356,64],[357,359,65],[360,362,66],[363,365,67],[366,368,68],[369,370,69],[371,373,70],[374,376,71],[377,379,72],[380,382,73],[383,386,74],[387,389,75],[390,391,76],[392,394,77],[395,399,78],[400,404,81],[405,409,82],[410,414,83],[415,419,84],[420,425,85],[426,439,86],[440,454,87],[455,475,88],[476,480,90],[481,490,91],[491,500,92],[501,510,93],[511,520,94],[521,529,95],[530,539,96],[540,549,97],[550,560,98],[561,569,99],[570,579,100],[580,589,102],[590,599,104],[600,610,106],[611,620,108],[621,630,110],[631,640,112],[641,650,114],[651,660,116],[661,670,117],[671,679,118],[680,690,119],[691,699,120]];
const bracketState={ready:false,type:"ap"},bracketEl={title:document.getElementById("bracketTitle"),current:document.getElementById("bracketCurrent"),goal:document.getElementById("bracketGoal"),currentLabel:document.getElementById("bracketCurrentLabel"),nextHint:document.getElementById("bracketNextHint"),requiredLabel:document.getElementById("bracketRequiredLabel"),required:document.getElementById("bracketRequired"),gainLabel:document.getElementById("bracketGainLabel"),gain:document.getElementById("bracketGain"),head:document.getElementById("bracketHead"),rows:document.getElementById("bracketRows")};
function bracketData(){return bracketState.type==="ap"?AP_BRACKETS:bracketState.type==="dp"?DP_BRACKETS:DR_BRACKETS}
function bracketUnit(){return bracketState.type==="ap"?"AP":"DP"}
function findBracket(value){
  const data=bracketData(),v=Number(value)||0;
  if(v<=data[0][0])return data[0];
  if(v>=data[data.length-1][1])return data[data.length-1];
  return data.find(row=>v>=row[0]&&v<=row[1])||data[data.length-1];
}
function nextBracket(value){const v=Number(value)||0;return bracketData().find(row=>row[0]>v)}
function renderBrackets(){if(!bracketEl.rows)return;const type=bracketState.type,cur=Number(bracketEl.current.value||0),goal=Number(bracketEl.goal.value||0),data=bracketData(),curRow=findBracket(cur),goalRow=findBracket(goal),next=nextBracket(cur),unit=bracketUnit();bracketEl.title.textContent=`${type.toUpperCase()} Brackets`;bracketEl.currentLabel.textContent=type==="ap"?"Your AP":"Your DP";bracketEl.requiredLabel.textContent=`${unit} Required`;bracketEl.gainLabel.textContent=type==="ap"?"AP gain":type==="dp"?"DP Gain":"DP Gain";bracketEl.nextHint.textContent=next?`${unit} for next bracket: ${next[0]-cur} ${unit}`:"Highest bracket reached";bracketEl.required.textContent=`${Math.max(0,goal-cur)} ${unit}`;bracketEl.gain.textContent=type==="ap"?`${Math.max(0,goalRow[2]-curRow[2])} AP`:type==="dp"?`${Math.max(0,goal-cur)} DP & ${Math.max(0,goalRow[2]-curRow[2])}% dr`:`${Math.max(0,goal-cur)} DP & ${Math.max(0,goalRow[2]-curRow[2])} dr`;bracketEl.head.innerHTML=type==="ap"?"<tr><th>AP</th><th>AP</th><th>Bonus AP</th><th>Bonus Diff.</th><th>Total Attack AP</th></tr>":type==="dp"?"<tr><th>DP</th><th>DP</th><th>Bonus % DR</th><th>Difference</th></tr>":"<tr><th>DP</th><th>DP</th><th>Damage Reduction</th></tr>";bracketEl.rows.innerHTML=data.map((r,i)=>{const active=cur>=r[0]&&cur<=r[1],prev=i?data[i-1][2]:0,diff=r[2]-prev,range=Math.min(12,Math.max(1,r[1]-r[0]+1));return type==="ap"?`<tr class="${active?"active":""}"><td>${r[0]}</td><td>${r[1]}</td><td>${r[2]}</td><td><span class="bracketBar" style="width:${Math.min(130,Math.max(28,diff*6))}px">${diff}</span></td><td><span class="bracketBar blue" style="width:${Math.min(150,Math.max(34,(r[1]+r[2])/5))}px">${r[1]+r[2]}</span></td></tr>`:type==="dp"?`<tr class="${active?"active":""}"><td>${r[0]}</td><td>${r[1]}</td><td>${r[2]} %</td><td><span class="bracketBar" style="width:${Math.max(28,range*13)}px">${r[1]-r[0]+1}</span></td></tr>`:`<tr class="${active?"active":""}"><td>${r[0]}</td><td>${r[1]}</td><td>${r[2]}</td></tr>`}).join("")}
function initializeBrackets(){if(bracketState.ready)return;bracketState.ready=true;document.querySelectorAll("[data-bracket-tab]").forEach(b=>b.addEventListener("click",()=>{bracketState.type=b.dataset.bracketTab;document.querySelectorAll("[data-bracket-tab]").forEach(x=>x.classList.toggle("active",x===b));if(bracketState.type==="ap"){bracketEl.current.value=378;bracketEl.goal.value=381}else{bracketEl.current.value=300;bracketEl.goal.value=320}renderBrackets()}));document.querySelectorAll("[data-bracket-step]").forEach(b=>b.addEventListener("click",()=>{const [id,delta]=b.dataset.bracketStep.split(":");const input=id==="current"?bracketEl.current:bracketEl.goal;input.value=Number(input.value||0)+Number(delta);renderBrackets()}));[bracketEl.current,bracketEl.goal].forEach(x=>x?.addEventListener("input",renderBrackets));renderBrackets()}

const MASTERY_LEVELS=[...Array.from({length:41},(_,i)=>i*50),...Array.from({length:20},(_,i)=>2050+i*50)];
const masteryState={ready:false,skill:"gathering",transition:null,current:Number(readSetting("masteryCalcCurrent",1000)),goal:Number(readSetting("masteryCalcGoal",1250))};
const masteryEl={tabs:document.getElementById("masterySkillTabs"),stage:document.getElementById("masteryTableStage"),head:document.getElementById("masteryHead"),rows:document.getElementById("masteryRows"),info:document.getElementById("masteryInfo"),extra:document.getElementById("masteryExtra"),rarity:document.getElementById("masteryRarityStrip"),current:document.getElementById("masteryCurrentInput"),goal:document.getElementById("masteryGoalInput"),currentBracket:document.getElementById("masteryCurrentBracket"),nextBracket:document.getElementById("masteryNextBracket"),needed:document.getElementById("masteryNeeded"),bonusPreview:document.getElementById("masteryBonusPreview")};
const pct=(n,plus=false)=>`${plus&&n>0?"+":""}${Number(n).toFixed(2)}%`;
const clamp=(n,min,max)=>Math.max(min,Math.min(max,n));
function masteryCurve(m,max,exp=1.45){return m<=2000?max*.8*Math.pow(m/2000,exp):max*.8+(max*.2*((m-2000)/1000))}
const processingRows=[[2,10],[20,11],[40,12],[60,13],[80,14],[100,15],[120,16],[140,17],[160,18],[180,19],[200,20],[220,21],[240,22],[260,23],[280,24],[300,25],[320,26],[340,27],[360,28],[380,29],[400,30],[420,31],[440,32],[460,33],[480,34],[500,35],[520,36],[540,37],[560,38],[580,39],[600,40],[620,41],[640,42],[660,43],[680,45],[700,47],[720,49],[740,51],[760,53],[780,57],[810,60],[840,64],[870,68],[900,72],[930,76],[960,80],[990,85],[1020,90],[1060,96],[1100,112],[1140,118],[1180,124],[1220,130],[1260,137],[1300,144],[1350,154],[1400,162],[1450,170],[1500,178],[1550,186],[1600,194],[1650,203],[1700,212],[1800,222],[1900,235],[2000,250],[2100,260],[2200,270],[2300,280],[2400,285],[2500,290],[2600,295],[2700,300],[2800,305],[2900,310],[3000,315]];
const MASTERY_CONFIG={
  gathering:{name:"Gathering",note:"Gathering mastery affects gathering item chances and amount increases.",columns:["Chance to get common item","Drop Amount Increase","Chance to get special items","Drop Amount Increase","Chance to get rare items","Drop Amount Increase","Chance to get very rare items","Drop Amount Increase"],rows:()=>MASTERY_LEVELS.map(m=>{const drop=masteryCurve(m,500,1.42),common=m<100?0:80;return [m,pct(common),pct(drop),pct(drop*.25),pct(drop*.125),pct(drop*.31),pct(drop*.104),pct(drop*.17),pct(drop*.064)]})},
  fishing:{name:"Fishing",note:"Fishing mastery affects prize catch rate and harpoon levels.",columns:["Prize Catch Fish Rate","Prize Catch Harpoon Level"],rows:()=>MASTERY_LEVELS.map(m=>[m,`+${pct(m<=2000?m/400:m/400+((m-2000)/1000)*1.25,false)}`,`+${Math.min(3,Math.floor((m+200)/500))}`])},
  hunting:{name:"Hunting",note:"Hunting mastery affects item grade chances and drop amount increases.",columns:["Chance to get common item","Drop Amount Increase","Chance to get special items","Drop Amount Increase","Chance to get rare items","Drop Amount Increase","Chance to get very rare items","Drop Amount Increase"],rows:()=>MASTERY_LEVELS.map(m=>{const drop=masteryCurve(m,400,1.36),common=m<100?0:80;return [m,pct(common),pct(drop),pct(drop*.5),pct(drop*.5),pct(drop*.62),pct(drop*.333),pct(drop*.36),pct(drop*.16)]})},
  alchemy:{name:"Alchemy",note:"Alchemy mastery affects production bonuses, mass production, and Imperial silver bonus.",columns:["Product Increase Amount","Chance to get common item","Chance to get special items","Chance to get rare items","Imperial Silver Bonus"],rows:()=>MASTERY_LEVELS.map(m=>[m,pct(masteryCurve(m,62.5,1.35)),pct(clamp(.25+masteryCurve(m,3.58,1.3),.25,3.83)),pct(clamp(.04+masteryCurve(m,2.94,1.4),.04,2.98)),pct(clamp(.01+masteryCurve(m,.35,1.5),.01,.36)),pct(imperialBonus(m))])},
  processing:{name:"Processing",note:"Processing mastery affects the material count used by mass processing.",columns:["Material Count"],rows:()=>processingRows},
  cooking:{name:"Cooking",note:"Cooking mastery affects production bonuses, mass cooking chance, and Imperial silver bonus.",columns:["Product Increase Amount","By Product Amount Increase","Rare Product Amount Increase","Mass Product Chance","Imperial Silver Bonus"],rows:()=>MASTERY_LEVELS.map(m=>[m,pct(masteryCurve(m,76.45,1.42)),pct(masteryCurve(m,76.45,1.42)),pct(masteryCurve(m,24.2,1.38)),pct(clamp(10+masteryCurve(m,90,1.22),0,100)),pct(imperialBonus(m))])},
  sailing:{name:"Sailing",note:"Sailing mastery affects ship control stats.",columns:["Accel.","Brake","Turn","Speed"],rows:()=>MASTERY_LEVELS.map(m=>{const v=m<=2000?m/100:m/200+10;return [m,pct(v),pct(v),pct(v),pct(v)]})},
  training:{name:"Training",note:"Training mastery affects capture rate, mount EXP, and breeding chance.",columns:["Capture Horse Rate","Mount Exp Gain","Breeding Chance"],rows:()=>MASTERY_LEVELS.map(m=>[m,pct(masteryCurve(m,43.75,1.45)),pct(masteryCurve(m,93.75,1.55)),pct(Math.floor(m/200)*1+(m>=50?1:0))])}
};
function imperialBonus(m){return m===0?0:m<=1250?clamp(1.85+Math.pow(m/1250,1.45)*94,0,95.84):clamp(95.84+((m-1250)/1750)*85.41,0,181.25)}
function masteryNumber(value,fallback=0){const parsed=Number(value);return Number.isFinite(parsed)?clamp(Math.round(parsed),0,3000):fallback}
function masteryRowsFor(config){return config.rows().map(row=>[Number(row[0]),...row.slice(1)]).filter(row=>Number.isFinite(row[0])).sort((a,b)=>a[0]-b[0])}
function masteryRowAt(rows,value){let match=rows[0]||null;for(const row of rows){if(row[0]<=value)match=row;else break;}return match}
function masteryNextRow(rows,value){return rows.find(row=>row[0]>value)||null}
function renderMasteryCalculator(){const config=MASTERY_CONFIG[masteryState.skill]||MASTERY_CONFIG.gathering;if(!masteryEl.current||!masteryEl.goal)return;const current=masteryNumber(masteryEl.current.value,masteryState.current),goal=masteryNumber(masteryEl.goal.value,masteryState.goal);masteryState.current=current;masteryState.goal=goal;if(String(current)!==masteryEl.current.value)masteryEl.current.value=String(current);if(String(goal)!==masteryEl.goal.value)masteryEl.goal.value=String(goal);persistSetting("masteryCalcCurrent",current);persistSetting("masteryCalcGoal",goal);const rows=masteryRowsFor(config),currentRow=masteryRowAt(rows,current),goalRow=masteryRowAt(rows,goal),next=masteryNextRow(rows,current);if(masteryEl.currentBracket)masteryEl.currentBracket.textContent=currentRow?`${currentRow[0]} ${config.name}`:"-";if(masteryEl.nextBracket)masteryEl.nextBracket.textContent=next?`${next[0]} (${next[0]-current} away)`:"Max reached";if(masteryEl.needed)masteryEl.needed.textContent=goal>current?`${goal-current} mastery`:"Goal reached";if(masteryEl.bonusPreview){const preview=goalRow||currentRow;const summary=config.columns.slice(0,Math.min(4,config.columns.length)).map((label,index)=>`<span><b>${escapeHtml(label)}</b>${escapeHtml(preview?.[index+1]??"-")}</span>`).join("");masteryEl.bonusPreview.innerHTML=`<strong>${escapeHtml(config.name)} goal preview</strong><div>${summary}</div>`}}
function renderMasteryBrackets(){const config=MASTERY_CONFIG[masteryState.skill]||MASTERY_CONFIG.gathering;if(!masteryEl.rows)return;masteryEl.info.textContent=config.note;masteryEl.info.classList.toggle("active",masteryEl.info.dataset.open==="true");masteryEl.rarity?.classList.toggle("active",masteryState.skill==="gathering");const dataColWidth=(92.8/config.columns.length).toFixed(4);masteryEl.head.innerHTML=`<colgroup><col class="masteryFirstCol">${config.columns.map(()=>`<col style="width:${dataColWidth}%">`).join("")}</colgroup><tr><th>Mastery</th>${config.columns.map(c=>`<th>${c}</th>`).join("")}</tr>`;masteryEl.rows.innerHTML=config.rows().map(row=>`<tr>${row.map(cell=>`<td>${cell}</td>`).join("")}</tr>`).join("");renderMasteryCalculator()}
function switchMasterySkill(skill){if(!MASTERY_CONFIG[skill]||skill===masteryState.skill)return;masteryState.skill=skill;document.querySelectorAll("[data-mastery-skill]").forEach(b=>b.classList.toggle("active",b.dataset.masterySkill===skill));clearTimeout(masteryState.transition);masteryEl.stage?.classList.add("fading");masteryState.transition=setTimeout(()=>{renderMasteryBrackets();masteryEl.stage?.classList.remove("fading");},220)}
function initializeMasteryBrackets(){if(masteryState.ready)return;masteryState.ready=true;if(masteryEl.current)masteryEl.current.value=String(masteryNumber(masteryState.current,1000));if(masteryEl.goal)masteryEl.goal.value=String(masteryNumber(masteryState.goal,1250));masteryEl.tabs?.addEventListener("click",event=>{const button=event.target.closest("[data-mastery-skill]");if(button)switchMasterySkill(button.dataset.masterySkill)});[masteryEl.current,masteryEl.goal].forEach(input=>input?.addEventListener("input",renderMasteryCalculator));masteryEl.extra?.addEventListener("click",()=>{masteryEl.info.dataset.open=masteryEl.info.dataset.open==="true"?"false":"true";renderMasteryBrackets()});renderMasteryBrackets()}

const LIGHTSTONE_SETS=[{"type":"combat","category":"Accuracy","name":"Focused","effects":["All Accuracy +12"],"lightstones":["Fire: Marked","Fire: Marked","Fire: Marked","Fire: Marked"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Accuracy","name":"Silent Rage","effects":["All Accuracy +9"],"lightstones":["Fire: Marked","Fire: Marked","Fire: Marked","-"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"AP","name":"I\u0027m Mad!","effects":["All AP +6"],"lightstones":["Fire: Rage","Fire: Rage","Fire: Rage","-"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"AP","name":"Savage","effects":["All AP +9"],"lightstones":["Fire: Rage","Fire: Rage","Fire: Rage","Fire: Rage"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"AP","name":"The Wild","effects":["Extra AP Against Monsters +9"],"lightstones":["Fire: Predation","Fire: Predation","Fire: Predation","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"AP","name":"The Wild: Demihumans","effects":["Extra Damage to Demihumans +15"],"lightstones":["Fire: Roar","Fire: Roar","Fire: Roar","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"AP","name":"The Wild: Edania","effects":["Extra AP Against Edanian Monsters +15"],"lightstones":["Fire: Twisted","Fire: Twisted","Fire: Twisted","Iridescent Lightstone"],"source":"Official NA/EU Edania patch note"},{"type":"combat","category":"AP","name":"The Wild: Humans","effects":["Extra AP Against Humans: +12","Extra AP Against Adventurers +12"],"lightstones":["Fire: Blight","Fire: Blight","Fire: Blight","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"AP","name":"The Wild: Kamasylvia","effects":["Extra Damage to Kamasylvian Monsters +15"],"lightstones":["Fire: Fallen","Fire: Fallen","Fire: Fallen","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Damage","name":"Crocodile\u0027s Tooth","effects":["Extra AP Against Monsters +7","Down Attack Damage +5%","Critical Hit Damage +2%"],"lightstones":["Fire: Predation","Fire: Predation","Fire: Ground","Fire: Strike"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Damage","name":"Deathblow","effects":["Extra AP Against Monsters +10","Critical Hit Rate +10%"],"lightstones":["Fire: Predation","Fire: Predation","Fire: Blade","Fire: Blade"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Damage","name":"Exceed","effects":["Self-obtainable Black Spirit\u0027s Rage +20%"],"lightstones":["Fire: Earthquake","Fire: Frenzy","Wind: Mind","Iridescent Lightstone"],"source":"Official Asia Adventurer\u0027s Guide"},{"type":"combat","category":"Damage","name":"Fast \u0026 Ragious","effects":["Black Spirit\u0027s Rage Recovery +1% every 10 sec"],"lightstones":["Earth: Mountain","Earth: Mountain","Wind: Mind","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Damage","name":"Vicious Shadows","effects":["Extra AP Against Monsters +7","Back Attack Damage +5%","Critical Hit Damage +2%"],"lightstones":["Fire: Predation","Fire: Predation","Fire: Shadows","Fire: Strike"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"All-Out-Attack","effects":["All Damage Reduction -15","All Evasion -25","All AP +20","Critical Hit Damage +3%"],"lightstones":["Fire: Rage","Fire: Rage","Fire: Strike","Fire: Strike"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Blurr","effects":["All Resistance -80%","All Damage Reduction +20","All Evasion +40"],"lightstones":["Earth: Iron Wall","Earth: Waves","Wind: Mind","Iridescent Lightstone"],"source":"Official Asia Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Boulder","effects":["Monster Damage Reduction +10","Debuff Resistance Against Monsters +50%"],"lightstones":["Earth: Fitted","Earth: Fitted","Earth: Mountain","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Centaurus","effects":["All Evasion +24"],"lightstones":["Earth: Waves","Earth: Waves","Earth: Waves","Earth: Waves"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Expose Weakness","effects":["All Damage Reduction -15","All Evasion -25","All Accuracy +30","Back Attack Damage +3%"],"lightstones":["Fire: Marked","Fire: Marked","Fire: Shadows","Fire: Shadows"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Fasting","effects":["Max HP -200","All Damage Reduction +5","Max MP/WP/SP +850"],"lightstones":["Earth: Iron Wall","Earth: Iron Wall","Wind: Mind","Wind: Mind"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Flying Marksmen","effects":["All Damage Reduction -15","All Evasion -25","All Accuracy +30","Air Attack Damage +3%"],"lightstones":["Fire: Marked","Fire: Marked","Fire: Aerial","Fire: Aerial"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Impregnable Fortress","effects":["Monster Damage Reduction +20","Damage from Monsters -3%"],"lightstones":["Earth: Veil","Earth: Veil","Earth: Veil","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Last Gatekeeper","effects":["All Damage Reduction +15","All Evasion +27","Max HP +150","Knockback/Floating Resistance +8%"],"lightstones":["Earth: Iron Wall","Earth: Waves","Wind: Heart","Earth: Roots"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Light-Footed","effects":["All Evasion +18"],"lightstones":["Earth: Waves","Earth: Waves","Earth: Waves","-"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Maiming","effects":["All Damage Reduction -15","All Evasion -25","All Accuracy +30","Critical Hit Damage +3%"],"lightstones":["Fire: Marked","Fire: Marked","Fire: Strike","Fire: Strike"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Merciless","effects":["All Damage Reduction -15","All Evasion -25","All AP +20","Down Attack Damage +3%"],"lightstones":["Fire: Rage","Fire: Rage","Fire: Ground","Fire: Ground"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Mind Focus","effects":["All Damage Reduction +15","All Evasion +30","Max MP/WP/SP +700","All Resistance +3%"],"lightstones":["Wind: Mind","Wind: Mind","Wind: Mind","Wind: Mind"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Olun\u0027s Descendant","effects":["All Damage Reduction +15","All Evasion +27","Max HP +150","All Resistance +3%"],"lightstones":["Earth: Iron Wall","Earth: Waves","Wind: Heart","Earth: Mountain"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Predation","effects":["All Damage Reduction +15","All Evasion +27","Max HP +150","Grapple Resistance +8%"],"lightstones":["Earth: Iron Wall","Earth: Waves","Wind: Heart","Earth: Sand"],"source":"Official Asia Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Protection","effects":["All Damage Reduction -50","Siege Weapon Damage Resistance +30%"],"lightstones":["Earth: Roots","Earth: Boulder","Earth: Swamp","Wind: Mind"],"source":"Official Asia Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Raised from the Dead","effects":["All Damage Reduction -15","All Evasion -25","All AP +20","Air Attack Damage +3%"],"lightstones":["Fire: Rage","Fire: Rage","Fire: Aerial","Fire: Aerial"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Reinforcing the Vanguard","effects":["Max HP +100","Monster Damage Reduction +15","Damage from Monsters -2%"],"lightstones":["Wind: Heart","Wind: Heart","Earth: Veil","Earth: Veil"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Rigid Shield","effects":["All Damage Reduction +18"],"lightstones":["Earth: Iron Wall","Earth: Iron Wall","Earth: Iron Wall","Earth: Iron Wall"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Self-Defense","effects":["All Damage Reduction +6","All Evasion +9","Max HP +50"],"lightstones":["Earth: Iron Wall","Earth: Waves","Wind: Heart","-"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Shell","effects":["All Damage Reduction +12"],"lightstones":["Earth: Iron Wall","Earth: Iron Wall","Earth: Iron Wall","-"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Steel Heart","effects":["Max HP +250"],"lightstones":["Wind: Heart","Wind: Heart","Wind: Heart","Wind: Heart"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Steel Shield","effects":["All Damage Reduction +12","All Evasion +18"],"lightstones":["Earth: Iron Wall","Earth: Iron Wall","Earth: Waves","Earth: Waves"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Sweep","effects":["All Damage Reduction -15","All Evasion -25","All Accuracy +30","Down Attack Damage +3%"],"lightstones":["Fire: Marked","Fire: Marked","Fire: Ground","Fire: Ground"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Veteran","effects":["All Damage Reduction +15","All Evasion +27","Max HP +150","Stun/Stiffness/Freezing Resistance +8%"],"lightstones":["Earth: Iron Wall","Earth: Waves","Wind: Heart","Earth: Swamp"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Warrior Bloodline","effects":["All Damage Reduction +15","All Evasion +27","Max HP +150","Knockdown/Bound Resistance +8%"],"lightstones":["Earth: Iron Wall","Earth: Waves","Wind: Heart","Earth: Boulder"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Watch Your Back","effects":["All Damage Reduction -15","All Evasion -25","All AP +20","Back Attack Damage +3%"],"lightstones":["Fire: Rage","Fire: Rage","Fire: Shadows","Fire: Shadows"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Defense","name":"Water of Life","effects":["Max HP +150"],"lightstones":["Wind: Heart","Wind: Heart","Wind: Heart","-"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Annihilating Shadows","effects":["Combat EXP +100%","Skill EXP +20%","Extra AP Against Humans: +9","Extra AP Against Adventurers +9"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Fire: Blight","Fire: Blight"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Battle Aid","effects":["Combat EXP +50%","Skill EXP +10%","Monster Damage Reduction +10"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Earth: Fitted","Earth: Fitted"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Building the Vanguard","effects":["Combat EXP +50%","Skill EXP +10%","Monster Damage Reduction +5"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Earth: Fitted","-"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Clearing Ranks","effects":["Combat EXP +50%","Skill EXP +10%","Extra AP Against Monsters +5","Extra Damage to Demihumans +4"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Fire: Predation","Fire: Roar"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Clearing Shadows","effects":["Combat EXP +50%","Skill EXP +10%","Extra AP Against Monsters +5","Extra AP Against Humans: +3","Extra AP Against Adventurers +3"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Fire: Predation","Fire: Blight"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Dedication","effects":["Combat EXP +300%"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Combat)","Wind: Alert (Combat)","Wind: Alert (Combat)"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Drills: Blight-Fallen","effects":["Combat EXP +50%","Skill EXP +10%","Extra AP Against Monsters +3","Extra AP Against Humans: +3","Extra AP Against Adventurers +3","Extra Damage to Kamasylvian Monsters +4"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Fire: Blight","Fire: Fallen"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Drills: Blight-Roar","effects":["Combat EXP +50%","Skill EXP +10%","Extra AP Against Monsters +3","Extra AP Against Humans: +3","Extra AP Against Adventurers +3","Extra Damage to Demihumans +4"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Fire: Blight","Fire: Roar"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Drills: Fallen-Roar","effects":["Combat EXP +50%","Skill EXP +10%","Extra AP Against Monsters +3","Extra Damage to Kamasylvian Monsters +4","Extra Damage to Demihumans +4"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Fire: Roar","Fire: Fallen"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Establishing Order","effects":["Combat EXP +50%","Skill EXP +10%","Extra AP Against Monsters +3","Extra Damage to Demihumans +4"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Fire: Roar","-"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Goddess\u0027 Aide","effects":["Combat EXP +50%","Skill EXP +10%","Extra AP Against Monsters +5","Extra Damage to Kamasylvian Monsters +4"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Fire: Predation","Fire: Fallen"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Goddess\u0027 Blessing","effects":["Combat EXP +100%","Skill EXP +20%","Extra Damage to Kamasylvian Monsters +11"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Fire: Fallen","Fire: Fallen"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Goddess\u0027 Song","effects":["Combat EXP +50%","Skill EXP +10%","Extra AP Against Monsters +3","Extra Damage to Kamasylvian Monsters +4"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Fire: Fallen","-"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Hunting Beasts","effects":["Combat EXP +100%","Skill EXP +20%","Extra Damage to Demihumans +11"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Fire: Roar","Fire: Roar"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Inner Peace","effects":["Combat EXP +100%","Skill EXP +20%","Monster Damage Reduction +15","Damage from Monsters -2%"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Earth: Veil","Earth: Veil"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Judging Evil","effects":["Combat EXP +50%","Skill EXP +10%","Extra AP Against Monsters +3","Extra AP Against Humans: +3","Extra AP Against Adventurers +3"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Fire: Blight","-"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Organizing Ranks","effects":["Combat EXP +50%","Skill EXP +10%","Monster Damage Reduction +10","Damage from Monsters -1%"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Earth: Fitted","Earth: Veil"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Prayer for Victory","effects":["Combat EXP +50%","Skill EXP +10%","Extra AP Against Monsters +1"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Fire: Predation","-"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Progress","effects":["Combat EXP +175%","Skill EXP +30%"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Combat)","Wind: Alert (Skill)","Wind: Alert (Skill)"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Training","effects":["Combat EXP +50%","Skill EXP +10%","Extra AP Against Monsters +1","Monster Damage Reduction +5"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Fire: Predation","Earth: Fitted"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Training: Demihumans","effects":["Combat EXP +50%","Skill EXP +10%","Extra AP Against Monsters +3","Extra Damage to Demihumans +4","Monster Damage Reduction +5"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Fire: Roar","Earth: Fitted"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Training: Humans","effects":["Combat EXP +50%","Skill EXP +10%","Extra AP Against Monsters +3","Extra AP Against Adventurers +3","Monster Damage Reduction +5"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Fire: Blight","Earth: Fitted"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Training: Kamasylvia","effects":["Combat EXP +50%","Skill EXP +10%","Extra AP Against Monsters +3","Extra Damage to Kamasylvian Monsters +4","Monster Damage Reduction +5"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Fire: Fallen","Earth: Fitted"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Way of the Fighter","effects":["Skill EXP +50%"],"lightstones":["Wind: Alert (Skill)","Wind: Alert (Skill)","Wind: Alert (Skill)","Wind: Alert (Skill)"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Well-prepared","effects":["Combat EXP +50%","Skill EXP +10%","Extra AP Against Monsters +2"],"lightstones":["Wind: Alert (Combat)","Wind: Alert (Skill)","Fire: Predation","Fire: Predation"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Leveling","name":"Wondrous Journey","effects":["Combat EXP +150%","Skill EXP +30%","Max Stamina +100","Chance to Gain Knowledge +20%","Weight Limit +70LT"],"lightstones":["Wind: Lungs","Wind: Lungs","Wind: Alert (Combat)","Wind: Alert (Skill)"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Behind the Shadow","effects":["All AP +8","All Accuracy +12","Max Stamina +75","Back Attack Damage +5%"],"lightstones":["Fire: Rage","Fire: Marked","Wind: Lungs","Fire: Shadows"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Blacksmith\u0027s Blessing","effects":["Extra AP Against Monsters +5","Monster Damage Reduction +5","Gear Durability Reduction Resistance +30%"],"lightstones":["Fire: Predation","Earth: Fitted","Wind: Feather","Wind: Fortune"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Choice \u0026 Focus","effects":["All AP -150","Item Drop Rate +30%","Movement Speed +2%"],"lightstones":["Fire: Rush","Wind: Feather","Wind: Fortune","Wind: Fortune"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Emergency Landing","effects":["Fall Damage -50%","Movement Speed +2%"],"lightstones":["Fire: Rush","Fire: Rush","Wind: Feather","Wind: Fortune"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Enhanced Focus","effects":["All AP +5","All Accuracy +16","Max Stamina +50"],"lightstones":["Fire: Rage","Fire: Marked","Wind: Lungs","Fire: Marked"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Flying Kick","effects":["All AP +8","All Accuracy +12","Max Stamina +75","Air Attack Damage +5%"],"lightstones":["Fire: Rage","Fire: Marked","Wind: Lungs","Fire: Aerial"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Golden Hand","effects":["Item Drop Rate +15%","Weight Limit +25LT"],"lightstones":["Wind: Feather","Wind: Feather","Wind: Fortune","Wind: Fortune"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Improved Breathing","effects":["All AP +5","All Accuracy +8","Max Stamina +100"],"lightstones":["Fire: Rage","Fire: Marked","Wind: Lungs","Wind: Lungs"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Know-It-All","effects":["Chance to Gain Knowledge +30%","Chance to Gain Higher-grade Knowledge +5%"],"lightstones":["Fire: Zeal","Fire: Claws","Wind: Feather","Wind: Fortune"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Lost Angel Wings","effects":["Karma Recovery +50%","Movement Speed +2%"],"lightstones":["Fire: Zeal","Fire: Claws","Fire: Rush","Wind: Fortune"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Marathon","effects":["Max Stamina +150"],"lightstones":["Wind: Lungs","Wind: Lungs","Wind: Lungs","Wind: Lungs"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Panting","effects":["Max HP -500","All Evasion +5","Max Stamina +250"],"lightstones":["Wind: Lungs","Wind: Lungs","Earth: Waves","Wind: Mind"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Skill Master","effects":["All AP +8","All Accuracy +12","Max Stamina +75","Extra All Special Attack Damage +2%"],"lightstones":["Fire: Rage","Fire: Marked","Wind: Lungs","Fire: Frenzy"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Sprint","effects":["Max Stamina +100"],"lightstones":["Wind: Lungs","Wind: Lungs","Wind: Lungs","-"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Stomping","effects":["All AP +8","All Accuracy +12","Max Stamina +75","Down Attack Damage +5%"],"lightstones":["Fire: Rage","Fire: Marked","Wind: Lungs","Fire: Ground"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Take Off!","effects":["Jump Height +50","Movement Speed +2%"],"lightstones":["Fire: Rush","Fire: Rush","Wind: Feather","Wind: Feather"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Target Openings","effects":["All AP +8","All Accuracy +12","Max Stamina +75","Critical Hit Damage +5%"],"lightstones":["Fire: Rage","Fire: Marked","Wind: Lungs","Fire: Strike"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Trained","effects":["Weight Limit +70LT"],"lightstones":["Wind: Feather","Wind: Feather","Wind: Feather","Wind: Feather"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Trained Fists","effects":["All AP +10","All Accuracy +8","Max Stamina +50"],"lightstones":["Fire: Rage","Fire: Marked","Wind: Lungs","Fire: Rage"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"combat","category":"Utility","name":"Trainee","effects":["All AP +3","All Accuracy +5","Max Stamina +30"],"lightstones":["Fire: Rage","Fire: Marked","Wind: Lungs","-"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Alchemy","name":"A Fragment of a Star, a Spoonful of the Moon","effects":["Alchemy Time -2 sec","Alchemy EXP +10%","Alchemy Mastery +20","Weight Limit +30LT"],"lightstones":["Flora: Time","Flora: Malleable","Wind: Feather","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Alchemy","name":"Ancient\u0027s Alchemy","effects":["Alchemy EXP +5%"],"lightstones":["Flora: Time","Flora: Time","Flora: Time"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Alchemy","name":"Choice \u0026 Focus: Alchemy","effects":["Alchemy Mastery -500","Alchemy EXP +35%"],"lightstones":["Flora: Time","Flora: Time","Wind: Mind","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Alchemy","name":"Collector of Shattered Stars","effects":["Alchemy EXP +8%"],"lightstones":["Flora: Time","Flora: Time","Flora: Time","Flora: Time"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Alchemy","name":"Dreamer","effects":["Alchemy Mastery +15"],"lightstones":["Flora: Malleable","Flora: Malleable","Flora: Malleable","Flora: Malleable"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Alchemy","name":"Imperial Alchemist","effects":["Alchemy Mastery +30"],"lightstones":["Flora: Malleable","Flora: Malleable","Flora: Malleable","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Alchemy","name":"Mysterious Alchemist","effects":["Alchemy EXP +16%"],"lightstones":["Flora: Time","Flora: Time","Flora: Time","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Alchemy","name":"New Reagent","effects":["Alchemy Mastery +8"],"lightstones":["Flora: Malleable","Flora: Malleable","Flora: Malleable"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Barter","name":"Barter Scholar","effects":["Barter EXP +8%"],"lightstones":["Flora: Haggler","Flora: Haggler","Flora: Haggler","Flora: Haggler"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Barter","name":"Master Negotiator","effects":["Barter EXP +16%"],"lightstones":["Flora: Haggler","Flora: Haggler","Flora: Haggler","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Barter","name":"Scale Owner","effects":["Barter EXP +5%"],"lightstones":["Flora: Haggler","Flora: Haggler","Flora: Haggler"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Cooking","name":"Best Cooking-ever","effects":["Cooking Mastery +15"],"lightstones":["Flora: Stir","Flora: Stir","Flora: Stir","Flora: Stir"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Cooking","name":"Choice \u0026 Focus: Cooking","effects":["Cooking Mastery -500","Cooking EXP +35%"],"lightstones":["Flora: Secret","Flora: Secret","Wind: Mind","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Cooking","name":"Excellent Chef","effects":["Cooking EXP +16%"],"lightstones":["Flora: Secret","Flora: Secret","Flora: Secret","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Cooking","name":"Feast of Flavors","effects":["Cooking Mastery +8"],"lightstones":["Flora: Stir","Flora: Stir","Flora: Stir"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Cooking","name":"Fundamentals of Cooking","effects":["Cooking Time -2 sec","Cooking EXP +10%","Cooking Mastery +20","Weight Limit +30LT"],"lightstones":["Flora: Secret","Flora: Stir","Wind: Feather","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Cooking","name":"Great Chef","effects":["Cooking EXP +8%"],"lightstones":["Flora: Secret","Flora: Secret","Flora: Secret","Flora: Secret"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Cooking","name":"Imperial Chef","effects":["Cooking Mastery +30"],"lightstones":["Flora: Stir","Flora: Stir","Flora: Stir","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Cooking","name":"Witch\u0027s Cauldron","effects":["Cooking EXP +5%"],"lightstones":["Flora: Secret","Flora: Secret","Flora: Secret"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Farming","name":"Cultivation","effects":["Farming EXP +8%"],"lightstones":["Flora: Harvest","Flora: Harvest","Flora: Harvest","Flora: Harvest"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Farming","name":"Fruit of Waiting","effects":["Farming EXP +5%"],"lightstones":["Flora: Harvest","Flora: Harvest","Flora: Harvest"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Farming","name":"Magical Soil","effects":["Farming EXP +16%"],"lightstones":["Flora: Harvest","Flora: Harvest","Flora: Harvest","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Fishing","name":"Best Stance","effects":["Fishing EXP +5%"],"lightstones":["Flora: Bite","Flora: Bite","Flora: Bite"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Fishing","name":"Big Catch","effects":["Fishing EXP +8%"],"lightstones":["Flora: Bite","Flora: Bite","Flora: Bite","Flora: Bite"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Fishing","name":"Choice \u0026 Focus: Fishing","effects":["Fishing Mastery -500","Fishing EXP +35%"],"lightstones":["Flora: Bite","Flora: Bite","Wind: Mind","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Fishing","name":"Dancing Fish","effects":["Fishing Mastery +15"],"lightstones":["Flora: Patience","Flora: Patience","Flora: Patience","Flora: Patience"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Fishing","name":"Fisher\u0027s Secrets","effects":["Fishing Mastery +30"],"lightstones":["Flora: Patience","Flora: Patience","Flora: Patience","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Fishing","name":"Nibbles","effects":["Auto-fishing Time -15%","Fishing EXP +10%","Fishing Mastery +20","Fishing +1"],"lightstones":["Flora: Bite","Flora: Patience","Wind: Lungs","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Fishing","name":"Reflexes","effects":["Fishing Mastery +8"],"lightstones":["Flora: Patience","Flora: Patience","Flora: Patience"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Fishing","name":"Seasoned Fisher","effects":["Fishing EXP +16%"],"lightstones":["Flora: Bite","Flora: Bite","Flora: Bite","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Fishing","name":"Sharp-eyed Seagull","effects":["Chance to Catch Rare Fish +5% Fishing EXP +10%","Fishing Mastery +20","Fishing +1"],"lightstones":["Flora: Bite","Flora: Patience","Wind: Fortune","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Fishing","name":"Whaling","effects":["Chance to Catch High-Quality Fish +6%","Fishing EXP +10%","Fishing Mastery +20","Fishing +1"],"lightstones":["Flora: Bite","Flora: Patience","Wind: Feather","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Gathering","name":"Choice \u0026 Focus: Gathering","effects":["Gathering Mastery -500","Gathering EXP +35%"],"lightstones":["Flora: Plains","Flora: Plains","Wind: Mind","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Gathering","name":"Curious","effects":["Gathering EXP +5%"],"lightstones":["Flora: Plains","Flora: Plains","Flora: Plains"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Gathering","name":"Four-leaf Clover","effects":["Gathering Mastery +8"],"lightstones":["Flora: Forest","Flora: Forest","Flora: Forest"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Gathering","name":"Friend of Fairies","effects":["Gathering EXP +8%"],"lightstones":["Flora: Plains","Flora: Plains","Flora: Plains","Flora: Plains"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Gathering","name":"Nature-lover","effects":["Gathering Mastery +30"],"lightstones":["Flora: Forest","Flora: Forest","Flora: Forest","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Gathering","name":"Pluck","effects":["Gathering EXP +16%"],"lightstones":["Flora: Plains","Flora: Plains","Flora: Plains","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Gathering","name":"Scurrying Weasel","effects":["Gathering Item Drop Rate +5%","Gathering EXP +10%","Gathering Mastery +20","Movement Speed +3%","Gathering +1"],"lightstones":["Flora: Plains","Flora: Forest","Fire: Rush","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Gathering","name":"Spirit\u0027s Echo","effects":["Gathering Mastery +15"],"lightstones":["Flora: Forest","Flora: Forest","Flora: Forest","Flora: Forest"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Gathering","name":"Yawning Hedgehog","effects":["Gathering Item Drop Rate +10%","Gathering EXP +10%","Gathering Mastery +20","Gathering +1","Energy Recovery +1"],"lightstones":["Flora: Plains","Flora: Forest","Wind: Fortune","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Hunting","name":"Blink of an Eye","effects":["Matchlock Reload Speed +10% Hunting EXP +10%","Hunting Mastery +20","Attack Speed +1","Critical Hit +2"],"lightstones":["Flora: Trap","Flora: Track","Fire: Zeal","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Hunting","name":"Canine Tooth","effects":["Critical Hit Rate +8%","Hunting EXP +10%","Hunting Mastery +20","Attack Speed +2","Critical Hit +1"],"lightstones":["Flora: Trap","Flora: Track","Fire: Claws","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Hunting","name":"Choice \u0026 Focus: Hunting","effects":["Hunting Mastery -500","Hunting EXP +35%"],"lightstones":["Flora: Trap","Flora: Trap","Wind: Mind","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Hunting","name":"Closed Snare","effects":["Hunting Mastery +15"],"lightstones":["Flora: Track","Flora: Track","Flora: Track","Flora: Track"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Hunting","name":"Crouching Predator","effects":["Hunting EXP +7%","Hunting Mastery +15","Attack Speed +1","Critical Hit +1"],"lightstones":["Flora: Trap","Flora: Track","Fire: Zeal","Fire: Claws"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Hunting","name":"Eye of a Hawk","effects":["Hunting EXP +8%"],"lightstones":["Flora: Trap","Flora: Trap","Flora: Trap","Flora: Trap"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Hunting","name":"First Shot of Dawn","effects":["Hunting Mastery +8"],"lightstones":["Flora: Track","Flora: Track","Flora: Track"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Hunting","name":"Hunter\u0027s Instinct","effects":["Hunting EXP +16%"],"lightstones":["Flora: Trap","Flora: Trap","Flora: Trap","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Hunting","name":"Legendary Marksman","effects":["Hunting Mastery +30"],"lightstones":["Flora: Track","Flora: Track","Flora: Track","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Hunting","name":"Sculpture","effects":["Hunting EXP +5%"],"lightstones":["Flora: Trap","Flora: Trap","Flora: Trap"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Life EXP","name":"Delotia","effects":["Life EXP +17%"],"lightstones":["Flora: Wildlife","Flora: Wildlife","Wind: Fortune","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Life Mastery","name":"Fortress of Nature","effects":["Life EXP +12%","Life Skill Mastery +20"],"lightstones":["Flora: Wildlife","Flora: Paradise","Wind: Fortune","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Life Mastery","name":"Hand of Manos","effects":["Life Skill Mastery +30"],"lightstones":["Flora: Paradise","Flora: Paradise","Wind: Fortune","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Lifeskill","name":"Horses Over Flowers","effects":["Mount EXP +25%"],"lightstones":["Flora: Gallop","Flora: Steed","Flora: Steed","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Lifeskill","name":"Refreshing Dream","effects":["Energy Recovery +2"],"lightstones":["Flora: Plains","Flora: Trap","Flora: Harvest","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Processing","name":"Choice \u0026 Focus: Processing","effects":["Processing Mastery -500","Processing EXP +35%"],"lightstones":["Flora: Tool","Flora: Tool","Wind: Mind","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Processing","name":"Clang! Clang!","effects":["Processing Success Rate +20%","Processing EXP +10%","Processing Mastery +20","Weight Limit +30LT"],"lightstones":["Flora: Tool","Flora: Deft","Wind: Feather","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Processing","name":"Deep Footprints","effects":["Processing Mastery +15"],"lightstones":["Flora: Deft","Flora: Deft","Flora: Deft","Flora: Deft"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Processing","name":"Humming in the Workshop","effects":["Processing Mastery +8"],"lightstones":["Flora: Deft","Flora: Deft","Flora: Deft"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Processing","name":"Multi-purpose","effects":["Processing Mastery +30"],"lightstones":["Flora: Deft","Flora: Deft","Flora: Deft","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Processing","name":"Processed Hand","effects":["Processing EXP +16%"],"lightstones":["Flora: Tool","Flora: Tool","Flora: Tool","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Processing","name":"Raising Power","effects":["Processing EXP +8%"],"lightstones":["Flora: Tool","Flora: Tool","Flora: Tool","Flora: Tool"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Processing","name":"Skillful Hands","effects":["Processing EXP +5%"],"lightstones":["Flora: Tool","Flora: Tool","Flora: Tool"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Sailing","name":"Chasing the Sunset","effects":["Sailing EXP +16%"],"lightstones":["Flora: Uncharted","Flora: Uncharted","Flora: Uncharted","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Sailing","name":"Choice \u0026 Focus: Sailing","effects":["Sailing Mastery -500","Sailing EXP +35%"],"lightstones":["Flora: Uncharted","Flora: Uncharted","Wind: Mind","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Sailing","name":"Ocean\u0027s Embrace","effects":["Mermaid\u0027s Wish III","Sailing EXP +10%","Barter EXP +10%","Sailing Mastery +45"],"lightstones":["Flora: Uncharted","Flora: Haggler","Flora: Blue","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Sailing","name":"Over the Horizon","effects":["Sailing EXP +8%"],"lightstones":["Flora: Uncharted","Flora: Uncharted","Flora: Uncharted","Flora: Uncharted"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Sailing","name":"Rusty Anchor","effects":["Sailing Mastery +15"],"lightstones":["Flora: Blue","Flora: Blue","Flora: Blue","Flora: Blue"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Sailing","name":"Sailor\u0027s Muscle","effects":["Sailing Mastery +8"],"lightstones":["Flora: Blue","Flora: Blue","Flora: Blue"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Sailing","name":"Swaying Helmsman","effects":["Sailing EXP +5%"],"lightstones":["Flora: Uncharted","Flora: Uncharted","Flora: Uncharted"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Sailing","name":"The Great Tide","effects":["Sailing Mastery +30"],"lightstones":["Flora: Blue","Flora: Blue","Flora: Blue","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Trading","name":"Quick-witted","effects":["Trading EXP +8%"],"lightstones":["Flora: Wagon","Flora: Wagon","Flora: Wagon","Flora: Wagon"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Trading","name":"The Cycle","effects":["Trading EXP +16%"],"lightstones":["Flora: Wagon","Flora: Wagon","Flora: Wagon","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Trading","name":"Trader","effects":["Trading EXP +5%"],"lightstones":["Flora: Wagon","Flora: Wagon","Flora: Wagon"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Training","name":"Choice \u0026 Focus: Training","effects":["Training Mastery -500","Training EXP +35%"],"lightstones":["Flora: Gallop","Flora: Gallop","Wind: Mind","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Training","name":"Commune","effects":["Training EXP +5%"],"lightstones":["Flora: Gallop","Flora: Gallop","Flora: Gallop"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Training","name":"Golden Mane","effects":["Training Mastery +15"],"lightstones":["Flora: Steed","Flora: Steed","Flora: Steed","Flora: Steed"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Training","name":"Gone with the Wind","effects":["Mount EXP +5%","Training EXP +5%","Training Mastery +10"],"lightstones":["Flora: Gallop","Flora: Gallop","Flora: Steed","Flora: Steed"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Training","name":"Grand Prix Glory","effects":["Training Mastery +30"],"lightstones":["Flora: Steed","Flora: Steed","Flora: Steed","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Training","name":"Into the Sunset","effects":["Mount EXP +15%","Training EXP +10%","Training Mastery +20"],"lightstones":["Flora: Gallop","Flora: Gallop","Flora: Steed","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Training","name":"Light Trot","effects":["Training EXP +8%"],"lightstones":["Flora: Gallop","Flora: Gallop","Flora: Gallop","Flora: Gallop"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Training","name":"Lucky Crop","effects":["Training EXP +16%"],"lightstones":["Flora: Gallop","Flora: Gallop","Flora: Gallop","Iridescent Lightstone"],"source":"Official NA/EU Adventurer\u0027s Guide"},{"type":"lifeskill","category":"Training","name":"Rising Trainer","effects":["Training Mastery +8"],"lightstones":["Flora: Steed","Flora: Steed","Flora: Steed"],"source":"Official NA/EU Adventurer\u0027s Guide"}];
const lightstoneState={ready:false,type:null,transition:null,search:"",category:"",amplified:false};
const lightstoneEl={chooser:document.getElementById("lightstoneChooser"),stage:document.getElementById("lightstoneStage"),title:document.getElementById("lightstoneStageTitle"),subtitle:document.getElementById("lightstoneStageSubtitle"),grid:document.getElementById("lightstoneSetGrid"),pill:document.getElementById("lightstoneModePill"),search:document.getElementById("lightstoneSearch"),category:document.getElementById("lightstoneCategoryFilter"),summary:document.getElementById("lightstoneSummary"),amplified:document.getElementById("lightstoneAmplifiedToggle")};
const lightstoneSetsByType=new Map(["combat","lifeskill"].map(type=>[type,LIGHTSTONE_SETS.filter(set=>set.type===type).sort((a,b)=>a.category.localeCompare(b.category)||a.name.localeCompare(b.name))]));
const lightstoneSearchIndex=new WeakMap();
const AMPLIFIED_LIGHTSTONE_EFFECTS={"Fire: Rage":"All AP +4","Fire: Marked":"All Accuracy +8","Fire: Predation":"Extra AP Against Monsters +5","Fire: Blight":"Extra AP Against Adventurers +6, Extra AP Against Humans +6","Fire: Roar":"Extra AP Against Demihumans +7","Fire: Fallen":"Extra AP Against Kamasylvian Monsters +7","Fire: Frenzy":"All Special Attack Extra Damage +1%","Fire: Ground":"Down Attack Damage +1.5%","Fire: Aerial":"Air Attack Damage +1.5%","Fire: Shadows":"Back Attack Damage +1.5%","Fire: Strike":"Critical Hit Damage +1.5%","Fire: Zeal":"Attack/Casting Speed +2","Fire: Rush":"Movement Speed +2","Fire: Claws":"Critical Hit +2","Fire: Blade":"Critical Hit Rate +3%","Fire: Twisted":"Extra AP Against Edanian Monsters +7","Earth: Iron Wall":"All Damage Reduction +5","Earth: Waves":"All Evasion +10","Earth: Fitted":"Monster Damage Reduction +7","Earth: Veil":"Monster Damage Reduction Rate +1.5%","Earth: Mountain":"All Resistance +2%","Earth: Swamp":"Stun/Stiffness/Freezing Resistance +4%","Earth: Boulder":"Knockdown/Bound Resistance +4%","Earth: Roots":"Knockback/Floating Resistance +4%","Wind: Heart":"Max HP +100","Wind: Mind":"Max MP/WP/SP +100","Wind: Lungs":"Max Stamina +50","Wind: Alert (Combat)":"Combat EXP +50%","Wind: Alert (Skill)":"Skill EXP +25%","Wind: Feather":"Weight Limit +30LT","Wind: Fortune":"Luck +2"};
const LIGHTSTONE_COLOR_PALETTE=["#ff5f6d","#ffa94d","#ffd166","#7bd88f","#4dd4ac","#4cc9f0","#73a7ff","#b987ff","#ff7ad9","#f4a261","#a3e635","#38bdf8"];
function normalizeLightstoneName(stone){return String(stone||"").replace(/^Amplified\s+Lightstone of\s+/i,"").replace(/^Lightstone of\s+/i,"").replace(/\s*:\s*/g,": ").trim()}
function amplifiedLightstoneFor(stone){const key=normalizeLightstoneName(stone);const effect=AMPLIFIED_LIGHTSTONE_EFFECTS[key];return effect?{name:`Amplified Lightstone of ${key}`,effect}:null}
function lightstoneSetTags(set){return set.lightstones.some(stone=>amplifiedLightstoneFor(stone))?["Amplified"]:[]}
function lightstoneCategorySort(a,b){if(a==="Amplified")return -1;if(b==="Amplified")return 1;return a.localeCompare(b)}
function lightstoneColorForStone(stone){const key=normalizeLightstoneName(stone);if(!key||key==="-")return "#778391";let hash=0;for(let i=0;i<key.length;i++)hash=(hash*31+key.charCodeAt(i))>>>0;return LIGHTSTONE_COLOR_PALETTE[hash%LIGHTSTONE_COLOR_PALETTE.length]}
function renderLightstoneStone(stone){const amp=amplifiedLightstoneFor(stone),useAmp=lightstoneState.amplified&&amp,color=lightstoneColorForStone(stone),name=useAmp?amp.name:stone,ampLine=useAmp?`<span class="lightstoneAmpLine">${escapeHtml(amp.effect)}</span>`:"";return `<li class="lightstoneStone" style="--stone-color:${color}"><span class="lightstoneStoneName">${escapeHtml(name)}</span>${ampLine}</li>`}
function lightstoneTypeLabel(type){return type==="lifeskill"?"Lifeskill":"Combat"}
function getLightstoneSets(type){return lightstoneSetsByType.get(type)||[]}
function lightstoneMatches(set,query){if(!query)return true;let hay=lightstoneSearchIndex.get(set);if(!hay){const ampTerms=set.lightstones.flatMap(stone=>{const amp=amplifiedLightstoneFor(stone);return amp?["Amplified",amp.name,amp.effect]:[]});hay=[set.name,set.category,set.source,...lightstoneSetTags(set),...set.effects,...set.lightstones,...ampTerms].join(" ").toLowerCase();lightstoneSearchIndex.set(set,hay)}return hay.includes(query.toLowerCase())}
function renderLightstoneCategoryOptions(type){if(!lightstoneEl.category)return;const categories=[...new Set(getLightstoneSets(type).flatMap(set=>[set.category,...lightstoneSetTags(set)]))].sort(lightstoneCategorySort);const current=categories.includes(lightstoneState.category)?lightstoneState.category:"";lightstoneState.category=current;lightstoneEl.category.innerHTML='<option value="">All categories</option>'+categories.map(category=>`<option value="${escapeHtml(category)}" ${category===current?"selected":""}>${escapeHtml(category)}</option>`).join("")}
function renderLightstoneCard(set){const effects=set.effects.map(effect=>`<li>${escapeHtml(effect)}</li>`).join("");const stones=set.lightstones.map(renderLightstoneStone).join("");const tags=[set.category,...lightstoneSetTags(set)].map(tag=>`<span class="lightstoneTag ${tag==="Amplified"?"lightstoneAmpTag":""}">${escapeHtml(tag)}</span>`).join("");return `<article class="lightstoneSetCard"><div class="lightstoneSetTop"><h3>${escapeHtml(set.name)}</h3><div class="lightstoneTagRow">${tags}</div></div><div class="lightstoneBlock"><strong>Set effect</strong><ul class="lightstoneLines">${effects}</ul></div><div class="lightstoneBlock"><strong>${lightstoneState.amplified?"Required lightstones - amplified where available":"Required lightstones"}</strong><ul class="lightstoneLines">${stones}</ul></div></article>`}
function renderLightstoneSets(){if(!lightstoneEl.stage||!lightstoneEl.grid)return;const type=lightstoneState.type;document.querySelectorAll("[data-lightstone-choice]").forEach(button=>button.classList.toggle("active",button.dataset.lightstoneChoice===type));lightstoneEl.chooser?.classList.toggle("hidden",Boolean(type));lightstoneEl.stage.classList.toggle("active",Boolean(type));lightstoneEl.pill.textContent=type?`${lightstoneTypeLabel(type)} selected`:"Select a category";if(lightstoneEl.amplified)lightstoneEl.amplified.checked=lightstoneState.amplified;if(!type){lightstoneEl.grid.innerHTML="";if(lightstoneEl.summary)lightstoneEl.summary.textContent="";return;}renderLightstoneCategoryOptions(type);const label=lightstoneTypeLabel(type);const sets=getLightstoneSets(type);const filtered=sets.filter(set=>(!lightstoneState.category||set.category===lightstoneState.category||lightstoneSetTags(set).includes(lightstoneState.category))&&lightstoneMatches(set,lightstoneState.search));lightstoneEl.title.textContent=`${label} Lightstone Sets`;lightstoneEl.subtitle.textContent="";if(lightstoneEl.summary){const categoryText=lightstoneState.category?` in ${lightstoneState.category}`:"";const ampText=lightstoneState.amplified?`<span>Amplified mode on</span>`:"";lightstoneEl.summary.innerHTML=`<span>Showing ${filtered.length} of ${sets.length} ${label.toLowerCase()} sets${categoryText}</span>${ampText}${lightstoneState.search?`<span>Search: ${escapeHtml(lightstoneState.search)}</span>`:""}`}lightstoneEl.grid.innerHTML=filtered.length?filtered.map(renderLightstoneCard).join(""):`<div class="lightstoneEmpty">No lightstone sets match the current filters.</div>`}
function selectLightstoneCategory(type){if(!["lifeskill","combat"].includes(type))return;lightstoneState.type=type;lightstoneState.search="";lightstoneState.category="";if(lightstoneEl.search)lightstoneEl.search.value="";clearTimeout(lightstoneState.transition);lightstoneEl.stage?.classList.add("fading");lightstoneState.transition=setTimeout(()=>{renderLightstoneSets();requestAnimationFrame(()=>lightstoneEl.stage?.classList.remove("fading"));},120)}
function initializeLightstoneSets(){if(lightstoneState.ready)return;lightstoneState.ready=true;let searchTimer=null;document.querySelectorAll("[data-lightstone-choice]").forEach(button=>button.addEventListener("click",()=>selectLightstoneCategory(button.dataset.lightstoneChoice)));lightstoneEl.search?.addEventListener("input",()=>{clearTimeout(searchTimer);searchTimer=setTimeout(()=>{lightstoneState.search=lightstoneEl.search.value.trim();renderLightstoneSets()},100)});lightstoneEl.category?.addEventListener("change",()=>{lightstoneState.category=lightstoneEl.category.value;renderLightstoneSets()});lightstoneEl.amplified?.addEventListener("change",()=>{lightstoneState.amplified=lightstoneEl.amplified.checked;renderLightstoneSets()});renderLightstoneSets()}
// Home dashboard schedule/timer config. Europe/Berlin follows the live CET/CEST offset.
const HOME_SERVER_TIME_ZONE = "Europe/Berlin";
function serverTimeZoneLabel(date=new Date()){
  const part=new Intl.DateTimeFormat("en-US",{timeZone:HOME_SERVER_TIME_ZONE,timeZoneName:"short"}).formatToParts(date).find(item=>item.type==="timeZoneName");
  return part?.value||"EU";
}
const HOME_DAYS = ["Monday","Tuesday","Wednesday","Thursday","Friday","Saturday","Sunday"];
const HOME_DAY_LABELS = {Monday:"Mon",Tuesday:"Tue",Wednesday:"Wed",Thursday:"Thu",Friday:"Fri",Saturday:"Sat",Sunday:"Sun"};
const HOME_DAY_INDEX = {Mon:0,Tue:1,Wed:2,Thu:3,Fri:4,Sat:5,Sun:6};
const HOME_BOSS_TIMES = ["00:15","02:00","12:00","14:00","16:00","19:00","19:15","22:15","23:15"];
const HOME_BOSS_SCHEDULE = {
  Monday:{"00:15":["Uturi","Kutum"],"02:00":["Sangoon","Karanda"],"12:00":["Sangoon","Nouver"],"14:00":["Garmoth"],"16:00":["Uturi","Kutum"],"19:00":["Golden Pig King","Nouver"],"19:15":[],"22:15":["Bulgasal","Kzarka"],"23:15":["Garmoth"]},
  Tuesday:{"00:15":["Sangoon","Karanda"],"02:00":[],"12:00":["Bulgasal","Kutum"],"14:00":["Garmoth"],"16:00":["Golden Pig King","Nouver"],"19:00":["Uturi","Kzarka"],"19:15":[],"22:15":["Quint","Muraka"],"23:15":["Garmoth"]},
  Wednesday:{"00:15":["Golden Pig King","Kzarka"],"02:00":[],"12:00":["Sangoon","Karanda"],"14:00":["Garmoth"],"16:00":["Bulgasal","Offin"],"19:00":["Vell"],"19:15":[],"22:15":["Uturi","Nouver"],"23:15":["Garmoth"]},
  Thursday:{"00:15":["Uturi","Nouver"],"02:00":["Golden Pig King","Kzarka"],"12:00":[],"14:00":["Garmoth"],"16:00":["Sangoon","Karanda"],"19:00":["Bulgasal","Kutum"],"19:15":[],"22:15":["Quint","Muraka"],"23:15":["Garmoth"]},
  Friday:{"00:15":["Golden Pig King","Karanda"],"02:00":["Bulgasal","Nouver"],"12:00":["Uturi","Kutum"],"14:00":["Garmoth"],"16:00":["Bulgasal","Kzarka"],"19:00":["Sangoon","Offin"],"19:15":[],"22:15":["Golden Pig King","Kutum"],"23:15":["Garmoth"]},
  Saturday:{"00:15":["Bulgasal","Kzarka"],"02:00":["Uturi","Offin"],"12:00":["Golden Pig King","Nouver"],"14:00":["Garmoth"],"16:00":["Black Shadow"],"19:00":["Sangoon","Karanda"],"19:15":[],"22:15":[],"23:15":[]},
  Sunday:{"00:15":["Bulgasal","Nouver"],"02:00":["Golden Pig King","Kutum"],"12:00":["Uturi","Kzarka"],"14:00":["Garmoth"],"16:00":["Vell"],"19:00":[],"19:15":["Garmoth"],"22:15":["Sangoon","Karanda"],"23:15":["Garmoth"]}
};
const HOME_BOSSES = ["Garmoth","Vell","Quint","Muraka","Kutum","Karanda","Kzarka","Nouver","Offin","Sangoon","Uturi","Golden Pig King","Bulgasal","Black Shadow"];
const HOME_TIMER_CONFIG = {
  dayNight:{ cycleMinutes:240, nightMinutes:40, nightStartUtcIso:"2026-07-20T11:40:00Z" },
  imperial:{ label:"Imperial delivery", resetHours:[0,3,6,9,12,15,18,21] },
  barter:{ label:"Barter refresh", resetHours:[0,4,8,12,16,20] },
  region:"EU"
};
const RESET_TIMER_CONFIG = [
  { id:"daily", label:"Daily Reset", kind:"daily", hour:2, minute:0, detail:"Daily quests, login checks, and most account-wide daily content" },
  { id:"imperial", label:"Imperial Reset", kind:"hours", hours:[2,5,8,11,14,17,20,23], detail:"Imperial delivery and related delivery windows" },
  { id:"bsa", label:"BSA Reset", kind:"daily", hour:7, minute:0, detail:"Black Spirit's Adventure daily reset" },
  { id:"agris", label:"Agris Reset", kind:"daily", hour:8, minute:0, detail:"Agris Fever recovery reset" },
  { id:"barter", label:"Barter Reset", kind:"daily", hour:8, minute:0, detail:"Barter refresh window" },
  { id:"trading", label:"Trading Reset", kind:"hours", hours:[2,5,8,11,14,17,20,23], detail:"Trading market reset window" }
];
const RESET_TIMER_ICON_SVGS = Object.freeze({
  daily:'<svg viewBox="0 0 24 24"><path d="M21 12a9 9 0 1 1-2.64-6.36L21 8"></path><path d="M21 3v5h-5"></path></svg>',
  imperial:'<svg viewBox="0 0 24 24"><path d="m3.3 7 8.7 5 8.7-5"></path><path d="M12 22V12"></path><path d="M21 8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16Z"></path></svg>',
  bsa:'<svg viewBox="0 0 24 24"><rect x="3" y="3" width="18" height="18" rx="3"></rect><circle class="iconFill" cx="8" cy="8" r="1.25"></circle><circle class="iconFill" cx="16" cy="8" r="1.25"></circle><circle class="iconFill" cx="12" cy="12" r="1.25"></circle><circle class="iconFill" cx="8" cy="16" r="1.25"></circle><circle class="iconFill" cx="16" cy="16" r="1.25"></circle></svg>',
  agris:'<svg viewBox="0 0 24 24"><path d="M12 22c4.42 0 8-3.13 8-7 0-2.74-1.56-5.1-4.08-7.1.15 2.46-1.38 3.58-1.38 3.58.38-3.45-1.8-6.88-5.42-9.48.45 2.85-1.06 4.78-2.7 6.57C4.68 10.46 4 12.37 4 15c0 3.87 3.58 7 8 7Z"></path><path d="M9.5 18.5c0-2 1-3.5 2.5-5 1.5 1.5 2.5 3 2.5 5"></path></svg>',
  barter:'<svg viewBox="0 0 24 24"><circle cx="12" cy="5" r="3"></circle><path d="M12 8v13"></path><path d="M5 12H2a10 10 0 0 0 20 0h-3"></path><path d="m5 9-3 3 3 3"></path><path d="m19 9 3 3-3 3"></path></svg>',
  trading:'<svg viewBox="0 0 24 24"><path d="m8 3-4 4 4 4"></path><path d="M4 7h16"></path><path d="m16 21 4-4-4-4"></path><path d="M20 17H4"></path></svg>'
});
const resetTimerEl={grid:document.getElementById("resetTimersGrid"),status:document.getElementById("resetTimersStatus"),localTime:document.getElementById("resetLocalTime")};
const homeEl={
  head:document.getElementById("bossScheduleHead"),body:document.getElementById("bossScheduleBody"),nextBossTitle:document.getElementById("homeNextBossTitle"),nextBossTime:document.getElementById("homeNextBossTime"),nextBossSub:document.getElementById("homeNextBossSub"),dayNightValue:document.getElementById("homeDayNightValue"),dayNightSub:document.getElementById("homeDayNightSub"),dayNightProgress:document.getElementById("homeDayNightProgress"),guildBossValue:document.getElementById("homeGuildBossValue"),guildBossSub:document.getElementById("homeGuildBossSub"),guildBossDay:document.getElementById("homeGuildBossDay"),guildBossTime:document.getElementById("homeGuildBossTime"),guildBossSetSub:document.getElementById("homeGuildBossSetSub"),nextBossLine:document.getElementById("homeBossNextLine"),time12:document.getElementById("homeTime12"),time24:document.getElementById("homeTime24"),localTime:document.getElementById("homeLocalTime"),master:document.getElementById("bossMasterNotifications"),lead:document.getElementById("bossLeadTime"),sound:document.getElementById("bossSoundEnabled"),tts:document.getElementById("bossTtsEnabled"),guildNotify:document.getElementById("guildBossNotifications"),testTts:document.getElementById("bossTestTts"),testAlarm:document.getElementById("bossTestAlarm"),toggles:document.getElementById("bossToggleList"),status:document.getElementById("homeScheduleStatus")
};
function bossClass(name){return `boss-${String(name).toLowerCase().replace(/\s+/g,"-")}`}
function defaultBossSelection(){return HOME_BOSSES.reduce((acc,b)=>{acc[b]=true;return acc},{})}
function normalizedHomeSettings(){const saved=readSetting("homeSettings",{}),tts=saved.ttsEnabled===true;return {timeFormat:saved.timeFormat==="24"?"24":"12",showLocalTime:saved.showLocalTime===true,masterNotifications:saved.masterNotifications!==false,leadMinutes:Number(saved.leadMinutes||15),soundEnabled:tts?false:saved.soundEnabled!==false,ttsEnabled:tts,guildBossNotifications:saved.guildBossNotifications===true,bosses:{...defaultBossSelection(),...(saved.bosses||{})},notified:saved.notified||{}}}
function saveHomeSettings(settings){persistSetting("homeSettings",settings)}
function commitHomeSettings(settings){saveHomeSettings(settings);applyHomeSettings(settings)}
function fmtCountdown(ms){ms=Math.max(0,Math.floor(ms/1000));const h=Math.floor(ms/3600),m=Math.floor(ms%3600/60),s=ms%60;return `${String(h).padStart(2,"0")}:${String(m).padStart(2,"0")}:${String(s).padStart(2,"0")}`}
function fmtBossTime(time,format){const [h,m]=time.split(":").map(Number);if(format==="24")return `${String(h).padStart(2,"0")}:${String(m).padStart(2,"0")}`;const suffix=h>=12?"PM":"AM",hour=((h+11)%12)+1;return `${String(hour).padStart(2,"0")}:${String(m).padStart(2,"0")} ${suffix}`}
function fmtLocalTime(date,format){return date.toLocaleTimeString([],format==="24"?{hour:"2-digit",minute:"2-digit",hour12:false}:{hour:"2-digit",minute:"2-digit",hour12:true})}
function zonedParts(date=new Date(),timeZone=HOME_SERVER_TIME_ZONE){const parts=new Intl.DateTimeFormat("en-US",{timeZone,weekday:"short",year:"numeric",month:"2-digit",day:"2-digit",hour:"2-digit",minute:"2-digit",second:"2-digit",hourCycle:"h23"}).formatToParts(date);const map={};for(const part of parts){if(part.type!=="literal")map[part.type]=part.value}return {weekday:map.weekday,year:Number(map.year),month:Number(map.month),day:Number(map.day),hour:Number(map.hour),minute:Number(map.minute),second:Number(map.second)}}
function zonedOffsetMs(date,timeZone){const p=zonedParts(date,timeZone);const asUtc=Date.UTC(p.year,p.month-1,p.day,p.hour,p.minute,p.second);return asUtc-date.getTime()}
function zonedTimeToDate(timeZone,year,month,day,hour,minute){const utc=Date.UTC(year,month-1,day,hour,minute,0);let offset=zonedOffsetMs(new Date(utc),timeZone);let result=new Date(utc-offset);const nextOffset=zonedOffsetMs(result,timeZone);if(nextOffset!==offset)result=new Date(utc-nextOffset);return result}
function serverWeekMondayUtc(now=new Date()){const p=zonedParts(now,HOME_SERVER_TIME_ZONE);return Date.UTC(p.year,p.month-1,p.day)-HOME_DAY_INDEX[p.weekday]*86400000}
function serverDateFor(mondayUtc,dayIndex,weekOffset=0){const d=new Date(mondayUtc+(dayIndex+(weekOffset*7))*86400000);return {year:d.getUTCFullYear(),month:d.getUTCMonth()+1,day:d.getUTCDate()}}
function buildServerWeekSpawns(now=new Date(),weekOffset=0){const mondayUtc=serverWeekMondayUtc(now);const spawns=[];HOME_DAYS.forEach((day,dayIndex)=>{const daySchedule=HOME_BOSS_SCHEDULE[day]||{};HOME_BOSS_TIMES.forEach(time=>{const bosses=daySchedule[time]||[];if(!bosses.length)return;const [hour,minute]=time.split(":").map(Number);const p=serverDateFor(mondayUtc,dayIndex,weekOffset);spawns.push({serverDay:day,serverDayIndex:dayIndex,serverTime:time,bosses,date:zonedTimeToDate(HOME_SERVER_TIME_ZONE,p.year,p.month,p.day,hour,minute)})})});return spawns}
let bossSpawnCache={mondayUtc:null,spawns:[]};
function allBossSpawns(now=new Date()){const mondayUtc=serverWeekMondayUtc(now);if(bossSpawnCache.mondayUtc!==mondayUtc){bossSpawnCache={mondayUtc,spawns:[...buildServerWeekSpawns(now,0),...buildServerWeekSpawns(now,1)].sort((a,b)=>a.date-b.date)}}return bossSpawnCache.spawns}
function nextBossSpawn(now=new Date()){return allBossSpawns(now).find(x=>x.date>now)||null}
function localDayName(date){return HOME_DAYS[(date.getDay()+6)%7]}
function fmtSpawnDateTime(spawn,settings){if(!spawn)return "-";if(settings.showLocalTime)return `${localDayName(spawn.date)} ${fmtLocalTime(spawn.date,settings.timeFormat)} Local`;return `${spawn.serverDay} ${fmtBossTime(spawn.serverTime,settings.timeFormat)} ${serverTimeZoneLabel(spawn.date)}`}
function nextServerReset(resetHours,now=new Date()){const sorted=[...resetHours].sort((a,b)=>a-b);const p=zonedParts(now,HOME_SERVER_TIME_ZONE);for(let dayOffset=0;dayOffset<2;dayOffset++){for(const h of sorted){const candidate=zonedTimeToDate(HOME_SERVER_TIME_ZONE,p.year,p.month,p.day+dayOffset,h,0);if(candidate>now)return candidate}}return zonedTimeToDate(HOME_SERVER_TIME_ZONE,p.year,p.month,p.day+1,sorted[0]||0,0)}
function nextDailyServerReset(hour=0,minute=0,now=new Date()){const p=zonedParts(now,HOME_SERVER_TIME_ZONE);for(let dayOffset=0;dayOffset<2;dayOffset++){const candidate=zonedTimeToDate(HOME_SERVER_TIME_ZONE,p.year,p.month,p.day+dayOffset,hour,minute);if(candidate>now)return candidate}return zonedTimeToDate(HOME_SERVER_TIME_ZONE,p.year,p.month,p.day+1,hour,minute)}
function resetTimerTarget(config,now=new Date()){return config.kind==="hours"?nextServerReset(config.hours,now):nextDailyServerReset(config.hour,config.minute||0,now)}
function normalizedResetSettings(){const saved=readSetting("resetTimerSettings",{});return {showLocalTime:saved.showLocalTime===true,timeFormat:(saved.timeFormat==="24"||normalizedHomeSettings().timeFormat==="24")?"24":"12"}}
function saveResetSettings(settings){persistSetting("resetTimerSettings",settings)}
function resetTimerServerLabel(target,settings){const p=zonedParts(target,HOME_SERVER_TIME_ZONE),time=`${String(p.hour).padStart(2,"0")}:${String(p.minute).padStart(2,"0")}`;return settings.showLocalTime?`${fmtLocalTime(target,settings.timeFormat)} Local`:`${fmtBossTime(time,settings.timeFormat)} ${serverTimeZoneLabel(target)}`}
function ensureResetTimerCards(){
  if(!resetTimerEl.grid||resetTimerEl.grid.children.length===RESET_TIMER_CONFIG.length)return;
  resetTimerEl.grid.innerHTML=RESET_TIMER_CONFIG.map(config=>`<article class="resetTimerCard" data-reset-id="${config.id}"><div class="resetTimerTop"><span class="resetTimerTitle">${escapeHtml(config.label)}</span><span class="resetTimerIcon" aria-hidden="true">${RESET_TIMER_ICON_SVGS[config.id]||""}</span></div><strong class="resetTimerValue" data-reset-value></strong><div class="resetTimerSub"><span data-reset-next></span><br>${escapeHtml(config.detail)}</div><div class="resetTimerRule"></div></article>`).join("");
}
function renderResetTimers(settings=normalizedResetSettings()){
  if(!resetTimerEl.grid)return;
  if(resetTimerEl.localTime)resetTimerEl.localTime.checked=settings.showLocalTime;
  ensureResetTimerCards();
  const now=new Date();
  for(const config of RESET_TIMER_CONFIG){
    const card=resetTimerEl.grid.querySelector(`[data-reset-id="${config.id}"]`),target=resetTimerTarget(config,now);
    if(!card)continue;
    const value=card.querySelector("[data-reset-value]"),next=card.querySelector("[data-reset-next]");
    if(value)value.textContent=fmtCountdown(target-now);
    if(next)next.textContent=`Next reset: ${resetTimerServerLabel(target,settings)} - ${HOME_TIMER_CONFIG.region}`;
  }
  if(resetTimerEl.status)resetTimerEl.status.textContent=`Reset timers loaded - Last updated ${now.toLocaleTimeString([],{hour:"2-digit",minute:"2-digit"})}`;
}
function initializeResetTimers(){renderResetTimers(normalizedResetSettings())}
function dayNightState(now=new Date()){const cfg=HOME_TIMER_CONFIG.dayNight,cycle=cfg.cycleMinutes*60000,night=cfg.nightMinutes*60000,nightStart=Date.parse(cfg.nightStartUtcIso);const elapsed=((now.getTime()-nightStart)%cycle+cycle)%cycle;const isNight=elapsed<night;const remain=isNight?night-elapsed:cycle-elapsed;const progress=isNight?elapsed/night:(elapsed-night)/(cycle-night);return{state:isNight?"Night":"Day",remain,progress,next:isNight?"Day":"Night"}}
function guildBossSchedule(){const migrated=readSetting("guildBossAt","");let day=readSetting("guildBossDay",""),time=readSetting("guildBossTime","");if((day===""||!time)&&migrated){const old=new Date(migrated);if(!Number.isNaN(old.getTime())){day=String(old.getDay());time=`${String(old.getHours()).padStart(2,"0")}:${String(old.getMinutes()).padStart(2,"0")}`;persistSetting("guildBossDay",day);persistSetting("guildBossTime",time);persistSetting("guildBossAt","")}}return{day:String(day),time:String(time||"")}}
function guildBossTarget(now=new Date()){const schedule=guildBossSchedule(),day=Number(schedule.day),parts=schedule.time.match(/^(\d{2}):(\d{2})$/);if(!Number.isInteger(day)||day<0||day>6||!parts)return null;const target=new Date(now);target.setHours(Number(parts[1]),Number(parts[2]),0,0);const daysAhead=(day-now.getDay()+7)%7;target.setDate(now.getDate()+daysAhead);if(target<=now)target.setDate(target.getDate()+7);return{...schedule,date:target}}
function guildBossDayName(day){return ["Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"][Number(day)]||"Weekly"}
function updateGuildBossTimer(now=new Date()){const schedule=guildBossSchedule();if(homeEl.guildBossDay&&homeEl.guildBossDay.value!==schedule.day)homeEl.guildBossDay.value=schedule.day;if(homeEl.guildBossTime&&homeEl.guildBossTime.value!==schedule.time)homeEl.guildBossTime.value=schedule.time;const target=guildBossTarget(now);if(!target){if(homeEl.guildBossValue)homeEl.guildBossValue.textContent="--:--:--";if(homeEl.guildBossSub)homeEl.guildBossSub.textContent="Set the weekly guild boss day/time";if(homeEl.guildBossSetSub)homeEl.guildBossSetSub.textContent="Choose the weekly day and time";return;}const diff=target.date-now;homeEl.guildBossValue.textContent=fmtCountdown(diff);homeEl.guildBossSub.textContent=`Until ${guildBossDayName(target.day)} ${target.time}`;homeEl.guildBossSetSub.textContent=`Repeats every ${guildBossDayName(target.day)} at ${target.time}`}
function renderBossNames(bosses){return bosses.length?bosses.map(b=>`<span class="bossName ${bossClass(b)}">${escapeHtml(b)}</span>`).join(""):`<span class="bossDash">&mdash;</span>`}
function sameBossList(a,b){return (a||[]).join("|")===(b||[]).join("|")}
function renderServerBossSchedule(settings,next){homeEl.head.innerHTML=`<tr><th>${serverTimeZoneLabel()}</th>${HOME_BOSS_TIMES.map(t=>`<th>${fmtBossTime(t,settings.timeFormat)}</th>`).join("")}</tr>`;homeEl.body.innerHTML=HOME_DAYS.map(day=>`<tr><th>${HOME_DAY_LABELS[day]}</th>${HOME_BOSS_TIMES.map(time=>{const bosses=(HOME_BOSS_SCHEDULE[day]||{})[time]||[],isNext=next&&next.serverDay===day&&next.serverTime===time&&sameBossList(next.bosses,bosses);return `<td><div class="bossScheduleCell ${isNext?"next":""}">${renderBossNames(bosses)}</div></td>`}).join("")}</tr>`).join("")}
function renderLocalBossSchedule(settings,next){const week=buildServerWeekSpawns(new Date(),0);const localTimes=[...new Set(week.map(s=>`${String(s.date.getHours()).padStart(2,"0")}:${String(s.date.getMinutes()).padStart(2,"0")}`))].sort((a,b)=>{const [ah,am]=a.split(":").map(Number),[bh,bm]=b.split(":").map(Number);return ah*60+am-(bh*60+bm)});const grouped={};HOME_DAYS.forEach(d=>grouped[d]={});week.forEach(s=>{const day=localDayName(s.date),time=`${String(s.date.getHours()).padStart(2,"0")}:${String(s.date.getMinutes()).padStart(2,"0")}`;grouped[day][time]=(grouped[day][time]||[]).concat(s.bosses)});homeEl.head.innerHTML=`<tr><th>Local</th>${localTimes.map(t=>`<th>${fmtBossTime(t,settings.timeFormat)}</th>`).join("")}</tr>`;homeEl.body.innerHTML=HOME_DAYS.map(day=>`<tr><th>${HOME_DAY_LABELS[day]}</th>${localTimes.map(time=>{const bosses=grouped[day][time]||[],isNext=next&&localDayName(next.date)===day&&`${String(next.date.getHours()).padStart(2,"0")}:${String(next.date.getMinutes()).padStart(2,"0")}`===time&&sameBossList(next.bosses,bosses);return `<td><div class="bossScheduleCell ${isNext?"next":""}">${renderBossNames(bosses)}</div></td>`}).join("")}</tr>`).join("")}
function renderBossSchedule(settings){if(!homeEl.head||!homeEl.body)return;const next=nextBossSpawn();settings.showLocalTime?renderLocalBossSchedule(settings,next):renderServerBossSchedule(settings,next)}
function renderBossToggles(settings){if(!homeEl.toggles)return;homeEl.toggles.innerHTML=HOME_BOSSES.map(b=>`<label class="bossToggle ${settings.bosses[b]?"":"disabledBoss"}"><input type="checkbox" data-boss-toggle="${escapeHtml(b)}" ${settings.bosses[b]?"checked":""}><span class="bossName ${bossClass(b)}">${escapeHtml(b)}</span></label>`).join("")}
function applyHomeSettings(settings){if(homeEl.time12)homeEl.time12.classList.toggle("active",settings.timeFormat==="12");if(homeEl.time24)homeEl.time24.classList.toggle("active",settings.timeFormat==="24");if(homeEl.localTime)homeEl.localTime.checked=settings.showLocalTime;if(homeEl.master)homeEl.master.checked=settings.masterNotifications;if(homeEl.lead)homeEl.lead.value=String(settings.leadMinutes);if(homeEl.sound){homeEl.sound.checked=settings.soundEnabled;homeEl.sound.disabled=settings.ttsEnabled;homeEl.sound.closest(".bossSettingRow")?.classList.toggle("disabledBoss",settings.ttsEnabled)}if(homeEl.tts)homeEl.tts.checked=settings.ttsEnabled;if(homeEl.guildNotify)homeEl.guildNotify.checked=settings.guildBossNotifications;renderBossSchedule(settings);renderBossToggles(settings);updateHomeTimers(settings)}
function updateHomeTimers(settings=normalizedHomeSettings()){const now=new Date(),next=nextBossSpawn(now);if(next){const bossLabel=next.bosses.join(" + "),countdown=fmtCountdown(next.date-now);homeEl.nextBossTime.textContent=countdown;if(homeEl.nextBossTitle){homeEl.nextBossTitle.textContent=`Next Boss: ${bossLabel}`;homeEl.nextBossTitle.className=`homeTimerTitle ${next.bosses.map(bossClass).join(" ")}`}homeEl.nextBossSub.textContent=`${fmtSpawnDateTime(next,settings)} - ${HOME_TIMER_CONFIG.region}`;homeEl.nextBossLine.textContent=`Next: ${bossLabel} in ${countdown} (${fmtSpawnDateTime(next,settings)})`}const dn=dayNightState(now);homeEl.dayNightValue.textContent=dn.state;homeEl.dayNightSub.textContent=`${fmtCountdown(dn.remain)} until ${dn.next}`;homeEl.dayNightProgress.style.width=`${Math.round(dn.progress*100)}%`;updateGuildBossTimer(now);homeEl.status.textContent=`Schedule loaded - Last updated ${now.toLocaleTimeString([],{hour:"2-digit",minute:"2-digit"})}`}
const HOME_ALERT_STAGES=[30,15,10,5];
function alertStage(settings,delta,keyBase){const lead=Number(settings.leadMinutes||15);return [...HOME_ALERT_STAGES].sort((a,b)=>a-b).find(stage=>stage<=lead&&delta<=stage*60000&&!(settings.notified||{})[`${keyBase}|${stage}`])||null}
function alertLeadText(stage){const minutes=Math.max(1,Number(stage)||1);return minutes===1?"1 minute":`${minutes} minutes`}
function spokenBossList(bosses){if(bosses.length<=1)return bosses[0]||"Boss";if(bosses.length===2)return `${bosses[0]} and ${bosses[1]}`;return `${bosses.slice(0,-1).join(", ")}, and ${bosses.at(-1)}`}
function notificationKeyDate(key){const part=String(key).split("|").find(x=>!Number.isNaN(Date.parse(x)));return part?Date.parse(part):NaN}
function pruneHomeNotifications(settings){const cutoff=Date.now()-14*86400000;for(const k of Object.keys(settings.notified||{})){const ts=notificationKeyDate(k);if(!Number.isNaN(ts)&&ts<cutoff)delete settings.notified[k]}}
function sendHomeAlert(title,message,spokenText,settings){if(settings.ttsEnabled){bridgeCall("speakText",{text:spokenText}).catch(error=>{console.warn("[HomeBossNotify] TTS failed",error);NotificationService.ShowWarning("Text to speech could not be played. In-app alert shown.","Boss notification fallback")});NotificationService.ShowInfo(spokenText,title);return;}bridgeCall("showDesktopNotification",{title,message,sound:false}).then(()=>{if(settings.soundEnabled)return bridgeCall("playAlarmSound",{});}).catch(error=>{console.warn("[HomeBossNotify] Windows notification failed",error);NotificationService.ShowWarning("Windows desktop notification could not be sent. In-app alert shown.","Boss notification fallback")});NotificationService.ShowInfo(message,title)}
function checkBossNotifications(settings,now,next){if(!settings.masterNotifications||!next)return;const lead=settings.leadMinutes*60000,delta=next.date-now;if(delta<0||delta>lead)return;const allowed=next.bosses.filter(b=>settings.bosses[b]);if(!allowed.length)return;const keyBase=`${next.date.toISOString()}|${allowed.join("+")}`,stage=alertStage(settings,delta,keyBase);if(!stage)return;const key=`${keyBase}|${stage}`;console.debug("[HomeBossNotify]",{bosses:allowed,spawnUtc:next.date.toISOString(),deltaMs:delta,stageMinutes:stage,leadMinutes:settings.leadMinutes,serverTime:fmtSpawnDateTime(next,{...settings,showLocalTime:false}),localTime:fmtSpawnDateTime(next,{...settings,showLocalTime:true})});settings.notified={...settings.notified,[key]:true};pruneHomeNotifications(settings);saveHomeSettings(settings);const title=`${allowed.join(" + ")} spawning soon`,message=`${fmtSpawnDateTime(next,settings)} - ${stage} minute warning - ${HOME_TIMER_CONFIG.region}`,spoken=`${spokenBossList(allowed)} spawning in ${alertLeadText(stage)}.`;sendHomeAlert(title,message,spoken,settings)}
function checkGuildBossNotifications(settings,now){if(!settings.masterNotifications||!settings.guildBossNotifications)return;const target=guildBossTarget(now);if(!target)return;const lead=settings.leadMinutes*60000,delta=target.date-now;if(delta<0||delta>lead)return;const keyBase=`guild|${target.date.toISOString()}`,stage=alertStage(settings,delta,keyBase);if(!stage)return;const key=`${keyBase}|${stage}`;settings.notified={...settings.notified,[key]:true};pruneHomeNotifications(settings);saveHomeSettings(settings);const title="Guild bosses spawning soon",message=`${guildBossDayName(target.day)} ${target.time} - ${stage} minute warning - Weekly guild boss timer`,spoken=`Guild bosses spawning in ${alertLeadText(stage)}.`;sendHomeAlert(title,message,spoken,settings)}
function runBackgroundNotifications(){const settings=normalizedHomeSettings(),now=new Date(),next=nextBossSpawn(now);checkBossNotifications(settings,now,next);checkGuildBossNotifications(settings,now)}
window.__bdoRunBackgroundNotifications=runBackgroundNotifications;
function initializeHomeDashboard(){const settings=normalizedHomeSettings();applyHomeSettings(settings)}
homeEl.time12?.addEventListener("click",()=>{const s=normalizedHomeSettings();s.timeFormat="12";commitHomeSettings(s);NotificationService.ShowInfo("Boss schedule uses 12-hour time.","Home settings saved")});
homeEl.time24?.addEventListener("click",()=>{const s=normalizedHomeSettings();s.timeFormat="24";commitHomeSettings(s);NotificationService.ShowInfo("Boss schedule uses 24-hour time.","Home settings saved")});
homeEl.localTime?.addEventListener("change",()=>{const s=normalizedHomeSettings();s.showLocalTime=homeEl.localTime.checked;commitHomeSettings(s);NotificationService.ShowInfo(`Boss schedule uses ${s.showLocalTime?"local PC time":"EU server time"}.`,"Home settings saved")});
homeEl.master?.addEventListener("change",()=>{const s=normalizedHomeSettings();s.masterNotifications=homeEl.master.checked;s.notified={};commitHomeSettings(s);NotificationService.ShowInfo(`Boss desktop notifications ${s.masterNotifications?"enabled":"disabled"}.`,"Home settings saved")});
homeEl.lead?.addEventListener("change",()=>{const s=normalizedHomeSettings();s.leadMinutes=Number(homeEl.lead.value);s.notified={};commitHomeSettings(s);NotificationService.ShowInfo(`Boss warning time set to ${s.leadMinutes} minutes.`,"Home settings saved")});
homeEl.sound?.addEventListener("change",()=>{const s=normalizedHomeSettings();s.soundEnabled=homeEl.sound.checked;commitHomeSettings(s);NotificationService.ShowInfo(`Notification sound ${s.soundEnabled?"enabled":"disabled"}.`,"Home settings saved")});
homeEl.tts?.addEventListener("change",()=>{const s=normalizedHomeSettings();s.ttsEnabled=homeEl.tts.checked;if(s.ttsEnabled)s.soundEnabled=false;s.notified={};commitHomeSettings(s);NotificationService.ShowInfo(`TTS announcements ${s.ttsEnabled?"enabled. Normal notification sound disabled":"disabled"}.`,"Home settings saved")});
homeEl.guildNotify?.addEventListener("change",()=>{const s=normalizedHomeSettings();s.guildBossNotifications=homeEl.guildNotify.checked;s.notified={};commitHomeSettings(s);NotificationService.ShowInfo(`Guild boss notifications ${s.guildBossNotifications?"enabled":"disabled"}.`,"Home settings saved")});
homeEl.testTts?.addEventListener("click",()=>{const settings=normalizedHomeSettings(),next=nextBossSpawn();const allowed=next?.bosses?.filter(b=>settings.bosses[b])||next?.bosses||[];const text=next&&allowed.length?`${spokenBossList(allowed)} spawning in ${alertLeadText(settings.leadMinutes)}.`:"No upcoming boss found to test.";bridgeCall("speakText",{text}).then(()=>NotificationService.ShowInfo(text,"TTS test sent")).catch(error=>NotificationService.ShowError(error.message||"Could not play TTS test."))});
homeEl.testAlarm?.addEventListener("click",()=>{bridgeCall("playAlarmSound",{}).then(()=>NotificationService.ShowInfo("Alarm test sent.","Alarm test")).catch(error=>NotificationService.ShowError(error.message||"Could not play alarm test."))});
function saveGuildBossSchedule(){persistSetting("guildBossDay",homeEl.guildBossDay?.value||"");persistSetting("guildBossTime",homeEl.guildBossTime?.value||"");persistSetting("guildBossAt","");updateGuildBossTimer();NotificationService.ShowInfo("Weekly guild boss timer saved.","Home settings saved")}
homeEl.guildBossDay?.addEventListener("change",saveGuildBossSchedule);
homeEl.guildBossTime?.addEventListener("change",saveGuildBossSchedule);
homeEl.toggles?.addEventListener("change",event=>{const input=event.target.closest("[data-boss-toggle]");if(!input)return;const s=normalizedHomeSettings();s.bosses[input.dataset.bossToggle]=input.checked;s.notified={};commitHomeSettings(s);NotificationService.ShowInfo(`${input.dataset.bossToggle} notifications ${input.checked?"enabled":"disabled"}.`,"Home settings saved")});
resetTimerEl.localTime?.addEventListener("change",()=>{const settings=normalizedResetSettings();settings.showLocalTime=resetTimerEl.localTime.checked;saveResetSettings(settings);renderResetTimers(settings);NotificationService.ShowInfo(`Reset timers now show ${settings.showLocalTime?"local PC time":"EU server time"}.`,"Reset timers saved")});

const GRIND_SPOTS=Array.isArray(window.BDO_GRIND_SPOTS)?window.BDO_GRIND_SPOTS:[];
const grindColors=["#60a5fa","#fbbf24","#818cf8","#fb7185","#34d399","#f472b6","#a78bfa","#22d3ee","#f97316","#c084fc","#4ade80","#e879f9"];
const grindState={initialized:false,mode:readSetting("grindTrackerMode","spot"),range:readSetting("grindTrackerRange","all"),selectedSpotId:String(readSetting("grindTrackerSelectedSpot",GRIND_SPOTS[0]?.id||"")),marketRegion:"eu",priceCache:readSetting("grindTrackerMarketPriceCache",{}),loadingPrices:false,pricePromise:null,priceTimer:null,sessions:null,storageLoadStarted:false,storageLoaded:false,spotSessionPage:0};
const grindDraft={reviewReady:false};
let grindPickerReturnFocus=null;
const grindEl={totalSilver:document.getElementById("grindTotalSilver"),averageHour:document.getElementById("grindAverageHour"),totalHours:document.getElementById("grindTotalHours"),hoursSummary:document.getElementById("grindHoursSummary"),silverSummary:document.getElementById("grindSilverSummary"),hourlySummary:document.getElementById("grindHourlySummary"),recentSummary:document.getElementById("grindRecentSummary"),hoursLegend:document.getElementById("grindHoursLegend"),hoursDonut:document.getElementById("grindHoursDonut"),silverBars:document.getElementById("grindSilverBars"),hourlyBars:document.getElementById("grindHourlyBars"),recent:document.getElementById("grindRecentSessions"),spotDetail:document.getElementById("grindSpotDetail"),add:document.getElementById("grindAddSession"),form:document.getElementById("grindSessionForm"),formTitle:document.getElementById("grindFormTitle"),id:document.getElementById("grindSessionId"),formSpot:document.getElementById("grindFormSpot"),formSpotSearch:document.getElementById("grindFormSpotSearch"),hours:document.getElementById("grindFormHours"),minutes:document.getElementById("grindFormMinutes"),className:document.getElementById("grindFormClass"),dropRate:document.getElementById("grindFormDropRate"),agris:document.getElementById("grindFormAgris"),lootInputs:document.getElementById("grindLootInputs"),buffs:document.getElementById("grindBuffPicker"),cancel:document.getElementById("grindCancelEdit"),start:document.getElementById("grindStartSession"),draftStatus:document.getElementById("grindDraftStatus"),imagePreview:document.getElementById("grindImagePreview")};
const GRIND_CLASS_OPTIONS=["Warrior","Ranger","Sorceress","Berserker","Tamer","Musa","Maehwa","Valkyrie","Kunoichi","Ninja","Wizard","Witch","Striker","Mystic","Lahn","Archer","Dark Knight","Shai","Guardian","Hashashin","Nova","Sage","Corsair","Drakania","Woosa","Maegu","Scholar","Dosa","Deadeye","Wukong","Seraph"];
const GRIND_CLASS_ICON_MAP={"warrior":"Assets/GrindTracker/classes/warrior.png","ranger":"Assets/GrindTracker/classes/ranger.png","sorceress":"Assets/GrindTracker/classes/sorceress.png","berserker":"Assets/GrindTracker/classes/berserker.png","tamer":"Assets/GrindTracker/classes/tamer.png","musa":"Assets/GrindTracker/classes/musa.png","maehwa":"Assets/GrindTracker/classes/maehwa.png","valkyrie":"Assets/GrindTracker/classes/valkyrie.png","kunoichi":"Assets/GrindTracker/classes/kunoichi.png","ninja":"Assets/GrindTracker/classes/ninja.png","wizard":"Assets/GrindTracker/classes/wizard.png","witch":"Assets/GrindTracker/classes/witch.png","striker":"Assets/GrindTracker/classes/striker.png","mystic":"Assets/GrindTracker/classes/mystic.png","lahn":"Assets/GrindTracker/classes/lahn.png","archer":"Assets/GrindTracker/classes/archer.png","dark-knight":"Assets/GrindTracker/classes/dark-knight.png","shai":"Assets/GrindTracker/classes/shai.png","guardian":"Assets/GrindTracker/classes/guardian.png","hashashin":"Assets/GrindTracker/classes/hashashin.png","nova":"Assets/GrindTracker/classes/nova.png","sage":"Assets/GrindTracker/classes/sage.png","corsair":"Assets/GrindTracker/classes/corsair.png","drakania":"Assets/GrindTracker/classes/drakania.png","woosa":"Assets/GrindTracker/classes/woosa.png","maegu":"Assets/GrindTracker/classes/maegu.png","scholar":"Assets/GrindTracker/classes/scholar.png","dosa":"Assets/GrindTracker/classes/dosa.png","deadeye":"Assets/GrindTracker/classes/deadeye.png","wukong":"Assets/GrindTracker/classes/wukong.png","seraph":"Assets/GrindTracker/classes/seraph.png"};
function grindClassKey(value){return String(value||"").trim().toLowerCase().replace(/[^a-z0-9]+/g,"-").replace(/^-+|-+$/g,"")}
function grindClassIconHtml(value){const icon=GRIND_CLASS_ICON_MAP[grindClassKey(value)];return icon?`<img class="grindClassIcon" src="${escapeHtml(icon)}" alt="">`:""}
function grindClassBadgeHtml(value){const label=String(value||"Unspecified").trim()||"Unspecified";return`<span class="grindClassBadge">${grindClassIconHtml(label)}<span>${escapeHtml(label)}</span></span>`}
function grindGroupLabelHtml(name){return grindState.mode==="class"?grindClassBadgeHtml(name):escapeHtml(name)}
const GRIND_BUFF_OPTIONS=Array.from({length:40},(_,index)=>{const number=String(index+1).padStart(2,"0");return{id:`buff-${number}`,label:`Modifier ${number}`,icon:`Assets/GrindTracker/buffs/buff-${number}.png`}});
const GRIND_BUFF_IDS=new Set(GRIND_BUFF_OPTIONS.map(option=>option.id));
const GRIND_PRICE_REFRESH_MS=24*60*60*1000;
const GRIND_PRICE_RETRY_MS=30*60*1000;
const GRIND_PRICE_CACHE_VERSION=14;
const GRIND_PRICE_CHUNK_SIZE=80;
const GRIND_PRICE_CHUNK_DELAY_MS=0;
const GRIND_FIXED_ITEM_PRICES={"5960":500,"44181":504,"44194":800,"44266":7500,"44267":15000,"44304":50000,"44305":50000,"44306":50000,"44311":50000,"44378":2100,"44400":15500,"44411":35000,"44423":8000,"44446":15000,"44448":15000,"44450":38000,"44451":15000,"44454":18000,"44455":12000,"44456":107000,"44476":4520,"44477":1820,"44482":18500,"44484":16000,"44485":17000,"44486":18000,"44487":17500,"44488":16000,"44489":19000,"44490":20140,"44495":24350,"44496":5620,"44516":25120,"44518":52500,"44519":117700,"44520":39000,"44521":40000,"44522":32950,"44523":16990,"44524":38800,"44525":35880,"45981":16100,"56322":52500,"56323":117700,"56327":52571,"56328":20520,"56329":59415,"56334":96040,"56338":59415,"59797":18575,"59798":96750,"59799":20000,"59800":26190,"59801":17520,"59802":25207,"59826":4902,"59827":5960,"59828":57777,"59880":25190,"65328":35100,"65329":32900,"65330":45570,"65397":34270,"65398":31500,"65399":30900,"65400":32900,"dehkia-mirumok-tainted-wood-fragment":101500,"tainted-bronze-fragment":125900,"stars-end-corrupted-sanguine-crystal":155000,"sycraia-upper-destroyed-ancient-weapon-power-stone":2350,"sycraia-underwater-ancient-weapon-power-stone":18000,"hoof-of-forest-ronaros":13490,"edania-ancient-soldier-fragment":147630,"edania-lightlost-core":140600,"edania-chilled-soul-piece":105640,"edania-contaminated-coral-piece":116200,"edania-hardened-lava-chunk":126980,"edania-tainted-armor-fragment":100507,"origin-of-corruption":3000000000};
const GRIND_MARKET_ITEM_ID_OVERRIDES={"corrupted-gluttony-crystal":15741,"gluttony-crystal":821344,"edania-refined-essence-of-devouring":767338,"edania-refined-origin-of-hunger":767337,"edania-crimson-primordial-pigment-sovereign":767293,"edania-violet-primordial-pigment-sovereign":767294,"edania-violet-primordial-pigment-edana":767296,"edania-crimson-primordial-luster-sovereign":821341,"edania-violet-primordial-luster-sovereign":821342,"edania-violet-primordial-luster-edana":821343,"corrupt-oil-of-immortality":1178};
Object.assign(GRIND_FIXED_ITEM_PRICES,{"44300":3000,"44322":1000,"44324":12000,"44425":1750,"44426":1925,"44427":1820,"44428":1890,"44429":2030,"44431":2100,"44432":2100,"44434":3150,"44435":2120,"44436":3000,"44437":3240,"44438":3600,"44439":3000,"44440":3445,"44442":4320,"44443":8800,"44494":9870,"faded-dark-energy":597680,"edania-primordial-fragment":30000000,"edania-won-crystal-of-ruin":5000000,"edania-bon-crystal-of-ruin":7000000,"edania-jin-crystal-of-ruin":8000000,"edania-han-crystal-of-ruin":10000000,"edania-won-crystal-of-dusky-ruin":500000000,"edania-bon-crystal-of-dusky-ruin":700000000,"edania-jin-crystal-of-dusky-ruin":800000000,"edania-han-crystal-of-dusky-ruin":1000000000});
Object.assign(GRIND_FIXED_ITEM_PRICES,{"mossy-ancient-ruins-fragment":10000,"great-marnis-stone-forest-ronaros":2000000,"8126":3000000,"8133":10000000,"15668":1000000,"6393":100000,"6399":100000,"6400":100000,"8124":3000000,"8129":3000000,"8135":100000,"8145":100000,"40968":100000,"44243":30000,"44284":30000,"44350":50000,"44383":1000000,"44405":100000,"65770":30000,"65780":30000,"721002":3000,"721044":30000000,"752023":51000,"757451":16400,"757452":18000,"757454":13200,"757455":14800,"757460":16000,"757470":17800,"757471":19400,"757473":16000,"820040":50000});
delete GRIND_FIXED_ITEM_PRICES["5960"];
Object.assign(GRIND_MARKET_ITEM_ID_OVERRIDES,{"edania-distorted-fragment-of-origin":821317,"edania-silent-fragment-of-origin":821318,"edania-crystallized-energy-of-endtimes":821252,"edania-distorted-crystal-of-origin":761802,"edania-silent-crystal-of-origin":761803,"edania-herald-s-crystal":821250,"edania-flawless-herald-s-crystal":821251,"imperfect-lightstone-of-earth":766105,"imperfect-lightstone-of-wind":766106,"sycraia-shard":821347});
const GRIND_NO_VALUE_ITEM_IDS=new Set(["ancient-creatures-scale","edania-deboreka-accessories","any-artifact","faint-sycraia-s-memory","gentle-sycraia-s-memory","intense-sycraia-s-memory","radiant-sycraia-s-memory","sycraia-underwater-ruins-paint","al-yurads-ring-piece","marnis-research-box","sycrids-scale-piece","void-tainted-whispers","752530","66108","66106","66107","40760","65778","65331","65332","15713","8958","8956","8957","8959","40709","40758","66945","56335","56505","8428","44799","40708","40756","44501","40706","40762","40711","40752","65327","56284","45017","45013","45018","45014"]);
function grindNormalizeItemName(value){return String(value||"").toLowerCase().replace(/\[[^\]]+\]/g,"").replace(/[^a-z0-9]+/g," ").trim()}
function grindAllDrops(){const map=new Map();GRIND_SPOTS.forEach(spot=>(spot.drops||[]).forEach(drop=>{if(!map.has(String(drop.id)))map.set(String(drop.id),drop)}));return[...map.values()]}
function grindDropHasNoValue(drop){const id=String(drop?.id||""),name=String(drop?.name||"");return GRIND_NO_VALUE_ITEM_IDS.has(id)||/^event-/i.test(id)||/\[event\]/i.test(name)}
function grindDropMarketId(drop){const id=String(drop?.id||"");if(!id||grindDropHasNoValue(drop)||Object.prototype.hasOwnProperty.call(GRIND_FIXED_ITEM_PRICES,id))return"";if(/^\d+$/.test(id))return id;return GRIND_MARKET_ITEM_ID_OVERRIDES[id]?String(GRIND_MARKET_ITEM_ID_OVERRIDES[id]):""}
function grindMarketItemIds(){return[...new Set(grindAllDrops().map(grindDropMarketId).filter(Boolean).map(Number))]}
function grindMarketDropsForSpot(spotId){const spot=grindSpotById(spotId);return(spot?.drops||[]).filter(drop=>grindDropMarketId(drop))}
function grindMarketItemIdsForSpot(spotId){return[...new Set(grindMarketDropsForSpot(spotId).map(grindDropMarketId).filter(Boolean).map(Number))]}
function grindEnsurePriceNameIndex(cache){if(!cache.priceNames||typeof cache.priceNames!=="object")cache.priceNames={};Object.values(cache.prices||{}).forEach(record=>{const key=grindNormalizeItemName(record?.name);if(key&&record?.itemId&&!cache.priceNames[key])cache.priceNames[key]=String(record.itemId)});return cache.priceNames}
function grindRegionPriceCache(region=grindState.marketRegion){const cache=grindState.priceCache&&typeof grindState.priceCache==="object"?grindState.priceCache:{};const normalized="eu";if(cache.na)delete cache.na;if(!cache[normalized]||typeof cache[normalized]!=="object")cache[normalized]={updatedAt:"",attemptedAt:"",prices:{},priceNames:{},message:"",version:GRIND_PRICE_CACHE_VERSION};if(cache[normalized].version!==GRIND_PRICE_CACHE_VERSION){cache[normalized].attemptedAt="";cache[normalized].version=GRIND_PRICE_CACHE_VERSION}if(!cache[normalized].prices||typeof cache[normalized].prices!=="object")cache[normalized].prices={};grindEnsurePriceNameIndex(cache[normalized]);grindState.priceCache=cache;grindState.marketRegion="eu";return cache[normalized]}
function grindPersistPriceCache(){persistSetting("grindTrackerMarketPriceCache",grindState.priceCache)}
function grindCachedMarketPrice(id,region=grindState.marketRegion){const cache=grindRegionPriceCache(region);return cache.prices?.[String(id)]||null}
function grindCachedMarketPriceByName(name,region=grindState.marketRegion){const cache=grindRegionPriceCache(region),key=grindNormalizeItemName(name),itemId=key?cache.priceNames?.[key]:"";return itemId?cache.prices?.[String(itemId)]||null:null}
function grindPriceRecordForDrop(drop,region=grindState.marketRegion){const id=String(drop?.id||"");if(grindDropHasNoValue(drop))return null;if(Object.prototype.hasOwnProperty.call(GRIND_FIXED_ITEM_PRICES,id))return{itemId:id,price:Number(GRIND_FIXED_ITEM_PRICES[id])||0,source:"fixed-vendor",capturedUtc:"",region:"fixed"};const marketId=grindDropMarketId(drop),cached=marketId?grindCachedMarketPrice(marketId,region):null;if(cached&&Number(cached.price)>0)return cached;const byName=grindCachedMarketPriceByName(drop?.name,region);if(byName&&Number(byName.price)>0)return byName;return null}
function grindDropPriceText(drop){if(grindDropHasNoValue(drop))return"";const record=grindPriceRecordForDrop(drop);if(record&&Number(record.price)>0)return grindFormatSilver(record.price);return""}
function grindDropPriceClass(drop){return grindPriceRecordForDrop(drop)?"grindPriceLine":"grindPriceLine pending"}
function grindDropPriceLine(drop){if(grindDropHasNoValue(drop))return"";const text=grindDropPriceText(drop);return`<span class="${grindDropPriceClass(drop)}">${escapeHtml(text||"Price unavailable")}</span>`}
function grindDropPriceSmall(drop){if(grindDropHasNoValue(drop))return"";const text=grindDropPriceText(drop);return`<small class="${text?"":"pending"}">${escapeHtml(text||"Price unavailable")}</small>`}
function grindUpdateMarketRegionButtons(){grindState.marketRegion="eu";persistSetting("grindTrackerMarketRegion","eu");document.querySelectorAll("[data-grind-market-region]").forEach(button=>button.classList.toggle("active",(button.dataset.grindMarketRegion||"eu")==="eu"))}
function grindPriceCacheFresh(region=grindState.marketRegion){const cache=grindRegionPriceCache(region),updated=Date.parse(cache.updatedAt||""),ids=grindMarketItemIds(),complete=ids.length>0&&ids.every(id=>Number(cache.prices?.[String(id)]?.price)>0);return complete&&Number.isFinite(updated)&&Date.now()-updated<GRIND_PRICE_REFRESH_MS}
function grindPriceAttemptFresh(region=grindState.marketRegion){if(grindPriceCacheFresh(region))return true;const attempted=Date.parse(grindRegionPriceCache(region).attemptedAt||"");return Number.isFinite(attempted)&&Date.now()-attempted<GRIND_PRICE_RETRY_MS}
function grindDelay(ms){return new Promise(resolve=>setTimeout(resolve,ms))}
async function grindFetchMarketPrices(itemIds,{force=false,silent=true}={}){
  const ids=[...new Set((itemIds||[]).map(Number).filter(id=>Number.isFinite(id)&&id>0))];
  if(!ids.length)return null;
  const requestedRegion="eu",cache=grindRegionPriceCache(requestedRegion);
  if(!force&&grindPriceAttemptFresh(requestedRegion))return cache;
  if(grindState.loadingPrices)return grindState.pricePromise||cache;

  grindState.marketRegion="eu";
  grindState.loadingPrices=true;
  grindState.priceProgress={done:0,total:ids.length};
  cache.attemptedAt=new Date().toISOString();
  grindPersistPriceCache();

  grindState.pricePromise=(async()=>{
    let savedPrices=0,lastMessage="";
    try{
      for(let index=0;index<ids.length;index+=GRIND_PRICE_CHUNK_SIZE){
        const chunk=ids.slice(index,index+GRIND_PRICE_CHUNK_SIZE);
        try{
          const data=await bridgeCall("getGrindMarketPrices",{region:requestedRegion,itemIds:chunk});
          const target=grindRegionPriceCache("eu"),returnedPrices=Array.isArray(data?.prices)?data.prices:[];
          returnedPrices.forEach(price=>{
            if(!price?.itemId||!Number(price.price))return;
            const record={
              itemId:String(price.itemId),
              enhancement:Number(price.enhancement)||0,
              name:String(price.name||""),
              price:Number(price.price)||0,
              lowestListedPrice:price.lowestListedPrice??null,
              basePrice:price.basePrice??null,
              lastSoldPrice:price.lastSoldPrice??null,
              stock:price.stock??null,
              tradeCount:price.tradeCount??null,
              source:String(price.source||"market"),
              capturedUtc:price.capturedUtc||data.capturedUtc||new Date().toISOString(),
              region:"eu"
            };
            savedPrices++;
            target.prices[record.itemId]=record;
            const nameKey=grindNormalizeItemName(record.name);
            if(nameKey)target.priceNames[nameKey]=record.itemId;
          });
          if(data?.capturedUtc&&returnedPrices.length)target.updatedAt=data.capturedUtc;
          target.attemptedAt=new Date().toISOString();
          target.message=String(data?.message||"");
          lastMessage=target.message;
          grindPersistPriceCache();
          if(!returnedPrices.length&&!savedPrices)break;
        }catch(error){
          console.warn("[GrindTracker] market price chunk failed",error);
          lastMessage=error.message||"Could not refresh the market cache.";
          break;
        }
        grindState.priceProgress.done=Math.min(ids.length,index+chunk.length);
        if(index+GRIND_PRICE_CHUNK_SIZE<ids.length&&GRIND_PRICE_CHUNK_DELAY_MS>0)await grindDelay(GRIND_PRICE_CHUNK_DELAY_MS);
      }
      const target=grindRegionPriceCache(requestedRegion);
      if(savedPrices>0&&!target.updatedAt)target.updatedAt=new Date().toISOString();
      target.attemptedAt=new Date().toISOString();
      if(!silent)NotificationService.ShowInfo(savedPrices?`Cached ${savedPrices} market price${savedPrices===1?"":"s"}.`:lastMessage||"Market refresh attempted. Cached and fixed values remain available.","Grind market prices");
      return target;
    }catch(error){
      console.warn("[GrindTracker] market price refresh failed",error);
      if(!silent)NotificationService.ShowWarning(error.message||"Could not refresh market prices. Cached and fixed values remain available.","Grind market prices");
      return cache;
    }
  })();

  try{
    return await grindState.pricePromise;
  }finally{
    grindState.loadingPrices=false;
    grindState.pricePromise=null;
    grindState.priceProgress=null;
    grindRender();
  }
}
function grindRefreshMarketPrices(options={}){const region="eu",next={...options};grindState.marketRegion="eu";if(next.silent===false&&!grindPriceCacheFresh(region))next.force=true;return grindFetchMarketPrices(grindMarketItemIds(),next)}
function grindSchedulePriceRefresh(){clearInterval(grindState.priceTimer);grindState.priceTimer=setInterval(()=>grindRefreshMarketPrices({force:false,silent:true}),GRIND_PRICE_REFRESH_MS);if(!grindPriceAttemptFresh())setTimeout(()=>grindRefreshMarketPrices({force:false,silent:true}),900)}
function grindEnsureMarketPricesForSpot(spotId){const ids=grindMarketItemIdsForSpot(spotId),missing=ids.filter(id=>!grindCachedMarketPrice(id));return missing.length?grindFetchMarketPrices(missing,{force:true,silent:true}):Promise.resolve(grindRegionPriceCache())}
function grindNormalizeBuffIds(value){return(Array.isArray(value)?value:[]).map(id=>String(id||"")).filter(id=>GRIND_BUFF_IDS.has(id))}
function grindSessionDefaults(){const saved=readSetting("grindTrackerSessionDefaults",{});return{className:String(saved?.className??readSetting("grindTrackerDefaultClass","")??""),dropRate:Math.max(0,Number(saved?.dropRate??readSetting("grindTrackerDefaultDropRate",0))||0),agris:saved?.agris!==undefined?!!saved.agris:readSetting("grindTrackerDefaultAgris",false)===true,hours:Math.max(0,Number(saved?.hours??readSetting("grindTrackerDefaultHours",0))||0),minutes:Math.max(0,Math.min(59,Number(saved?.minutes??readSetting("grindTrackerDefaultMinutes",0))||0)),buffs:grindNormalizeBuffIds(saved?.buffs??readSetting("grindTrackerDefaultBuffs",[]))}}
function grindClassControlNodes(){return{picker:document.getElementById("grindClassPicker"),button:document.getElementById("grindClassButton"),menu:document.getElementById("grindClassMenu")}}
function grindClassOptionHtml(value,selected){const label=value||"Select class";return`<button class="grindClassOption ${String(value)===String(selected)?"active":""}" type="button" role="option" aria-selected="${String(value)===String(selected)?"true":"false"}" data-grind-class-value="${escapeHtml(value)}">${value?grindClassBadgeHtml(value):`<span class="grindClassPlaceholder">${escapeHtml(label)}</span>`}</button>`}
function grindRenderClassPicker(){const nodes=grindClassControlNodes();if(!nodes.button||!nodes.menu)return;const selected=String(grindEl.className?.value||"").trim();nodes.button.innerHTML=selected?grindClassBadgeHtml(selected):`<span class="grindClassPlaceholder">Select class</span>`;nodes.menu.innerHTML=[grindClassOptionHtml("",selected),...GRIND_CLASS_OPTIONS.map(name=>grindClassOptionHtml(name,selected))].join("");nodes.button.setAttribute("aria-expanded",nodes.menu.hidden?"false":"true")}
function grindSetClassValue(value){if(!grindEl.className)return;const className=String(value||"").trim();if(className&&![...grindEl.className.options].some(option=>option.value===className))grindEl.className.insertAdjacentHTML("beforeend",`<option value="${escapeHtml(className)}">${escapeHtml(className)}</option>`);grindEl.className.value=className;grindRenderClassPicker()}
function grindPopulateClassSelect(){if(!grindEl.className)return;const selected=String(grindEl.className.value||grindSessionDefaults().className||"");grindEl.className.innerHTML=`<option value="">Select class</option>${GRIND_CLASS_OPTIONS.map(name=>`<option value="${escapeHtml(name)}">${escapeHtml(name)}</option>`).join("")}`;grindSetClassValue(selected)}
let grindClassPickerBound=false;
function grindToggleClassMenu(open){const nodes=grindClassControlNodes();if(!nodes.menu||!nodes.button)return;const next=open===undefined?nodes.menu.hidden:!!open;nodes.menu.hidden=!next;nodes.button.setAttribute("aria-expanded",next?"true":"false")}
function grindBindClassPicker(){if(grindClassPickerBound)return;grindClassPickerBound=true;const nodes=grindClassControlNodes();nodes.button?.addEventListener("click",event=>{event.stopPropagation();grindToggleClassMenu()});nodes.menu?.addEventListener("click",event=>{const option=event.target.closest("[data-grind-class-value]");if(!option)return;grindSetClassValue(option.dataset.grindClassValue||"");grindToggleClassMenu(false);grindPersistCurrentSessionDefaults()});document.addEventListener("click",event=>{if(!event.target.closest("#grindClassPicker"))grindToggleClassMenu(false)});document.addEventListener("keydown",event=>{if(event.key==="Escape")grindToggleClassMenu(false)})}
function grindSelectedBuffIds(){return[...(grindEl.buffs?.querySelectorAll("[data-grind-buff].active")||[])].map(button=>button.dataset.grindBuff).filter(id=>GRIND_BUFF_IDS.has(id))}
function grindSetBuffSelection(ids=[]){const selected=new Set(grindNormalizeBuffIds(ids));grindEl.buffs?.querySelectorAll("[data-grind-buff]").forEach(button=>{const active=selected.has(button.dataset.grindBuff);button.classList.toggle("active",active);button.setAttribute("aria-pressed",active?"true":"false")})}
function grindRenderBuffPicker(){if(!grindEl.buffs)return;grindEl.buffs.innerHTML=GRIND_BUFF_OPTIONS.map(option=>`<button class="grindBuffButton" data-grind-buff="${escapeHtml(option.id)}" type="button" aria-pressed="false" title="${escapeHtml(option.label)}"><img src="${escapeHtml(option.icon)}" alt="${escapeHtml(option.label)}"></button>`).join("");grindSetBuffSelection(grindSessionDefaults().buffs)}
function grindBuffsForSession(session){return grindNormalizeBuffIds(session?.buffs).map(id=>GRIND_BUFF_OPTIONS.find(option=>option.id===id)).filter(Boolean)}
function grindSessionBuffStrip(session,limit=12,empty="-"){const buffs=grindBuffsForSession(session);if(!buffs.length)return empty?`<span class="grindNoBuffs">${escapeHtml(empty)}</span>`:"";return`<span class="grindSessionBuffs">${buffs.slice(0,limit).map(buff=>`<img src="${escapeHtml(buff.icon)}" alt="${escapeHtml(buff.label)}" title="${escapeHtml(buff.label)}">`).join("")}${buffs.length>limit?`<span class="grindNoBuffs">+${buffs.length-limit}</span>`:""}</span>`}
function grindPersistCurrentSessionDefaults(){const defaults={className:String(grindEl.className?.value||"").trim(),dropRate:Math.max(0,Number(grindEl.dropRate?.value||0)||0),agris:grindCurrentAgris(),hours:Math.max(0,Number(grindEl.hours?.value||0)||0),minutes:Math.max(0,Math.min(59,Number(grindEl.minutes?.value||0)||0)),buffs:grindSelectedBuffIds()};persistSetting("grindTrackerSessionDefaults",defaults);persistSetting("grindTrackerDefaultClass",defaults.className);persistSetting("grindTrackerDefaultDropRate",defaults.dropRate);persistSetting("grindTrackerDefaultAgris",defaults.agris);persistSetting("grindTrackerDefaultHours",defaults.hours);persistSetting("grindTrackerDefaultMinutes",defaults.minutes);persistSetting("grindTrackerDefaultBuffs",defaults.buffs)}
function grindLocalDateTimeStamp(date=new Date()){const pad=value=>String(value).padStart(2,"0");return`${date.getFullYear()}-${pad(date.getMonth()+1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}:${pad(date.getSeconds())}`}
function grindSessionHours(session){return Math.max(0,Number(session.minutes||0)/60)}
function grindNormalizeSessions(rows){return Array.isArray(rows)?rows.filter(row=>row&&row.spotId&&Number(row.minutes)>0&&Number(row.silver)>=0):[]}
function grindLegacySessions(){return grindNormalizeSessions(readSetting("grindTrackerSessions",[]))}
function grindSessions(){if(Array.isArray(grindState.sessions))return grindState.sessions;grindState.sessions=grindLegacySessions();return grindState.sessions}
function grindRecoverySessions(){const current=grindNormalizeSessions(readSetting("grindTrackerSessionsRecovery",[]));if(current.length)return current;try{const legacy=grindNormalizeSessions(JSON.parse(localStorage.getItem("grindTrackerSessionsRecovery")||"[]"));if(legacy.length){settingMemory.set("grindTrackerSessionsRecovery",legacy);localStorage.setItem("bdoMultiTool.grindTrackerSessionsRecovery",JSON.stringify(legacy));localStorage.removeItem("grindTrackerSessionsRecovery")}return legacy}catch{return[]}}
function grindPersistRecovery(rows){try{const recovery=rows.slice(0,250),json=JSON.stringify(recovery);if(json.length<1500000){settingMemory.set("grindTrackerSessionsRecovery",recovery);localStorage.setItem("bdoMultiTool.grindTrackerSessionsRecovery",json);localStorage.removeItem("grindTrackerSessionsRecovery")}}catch(error){console.warn("[GrindTracker] Could not update the local recovery copy.",error)}}
async function saveGrindSessions(rows){const normalized=grindNormalizeSessions(rows);grindState.sessions=normalized;grindPersistRecovery(normalized);try{await bridgeCall("saveGrindSessions",{sessions:normalized});localStorage.removeItem("grindTrackerSessions");return normalized}catch(error){console.error("[GrindTracker] Native session save failed.",error);throw new Error("The session could not be saved safely. Your current form has been kept open.")}}
async function grindLoadSessions(){if(grindState.storageLoadStarted)return;grindState.storageLoadStarted=true;const legacy=grindLegacySessions();try{const nativeRows=grindNormalizeSessions(await bridgeCall("loadGrindSessions"));grindState.sessions=nativeRows.length?nativeRows:legacy;if(!nativeRows.length&&legacy.length)await saveGrindSessions(legacy);else localStorage.removeItem("grindTrackerSessions");grindState.storageLoaded=true;grindRender()}catch(error){grindState.sessions=legacy.length?legacy:grindRecoverySessions();grindState.storageLoaded=true;console.error("[GrindTracker] Native session load failed; using the recovery copy.",error);NotificationService.ShowWarning("Saved sessions were loaded from the recovery copy because the primary session file could not be read.","Grind Tracker");grindRender()}}
function grindSpotById(id){return GRIND_SPOTS.find(spot=>String(spot.id)===String(id))||GRIND_SPOTS[0]||null}
function grindSpotLabel(spot){return spot?`${spot.name}${spot.zone?` - ${spot.zone}`:""}`:"Unknown spot"}
function grindSpotSearchKey(value){return norm(value).replace(/[^a-z0-9]+/g," ").trim()}
function grindSpotSearchHaystack(spot){return grindSpotSearchKey([spot?.name,spot?.zone,spot?.primaryTrash,spot?.drops?.map(drop=>drop.name).join(" ")].join(" "))}
function grindFormSpotMatches(spot,query){const needle=grindSpotSearchKey(query);if(!needle)return true;const haystack=grindSpotSearchHaystack(spot);return haystack.includes(needle)||haystack.replace(/\s+/g,"").includes(needle.replace(/\s+/g,""))}
function grindFormSpotSearchResults(query=grindEl.formSpotSearch?.value||""){return GRIND_SPOTS.slice().sort(grindSortSpots).filter(spot=>grindFormSpotMatches(spot,query))}
function grindPopulateFormSpotSelect(query=grindEl.formSpotSearch?.value||"",preferred=grindState.selectedSpotId){if(!grindEl.formSpot)return"";const spots=grindFormSpotSearchResults(query),current=String(preferred||grindEl.formSpot.value||grindState.selectedSpotId||"");if(!spots.length){grindEl.formSpot.innerHTML=`<option value="">No grind zones found</option>`;return""}grindEl.formSpot.innerHTML=spots.map(spot=>`<option value="${escapeHtml(spot.id)}">${escapeHtml(grindSpotLabel(spot))}</option>`).join("");const selected=spots.some(spot=>String(spot.id)===current)?current:String(spots[0].id);grindEl.formSpot.value=selected;return selected}
function grindFilterFormSpotSelect(){const previous=String(grindEl.formSpot?.value||""),selected=grindPopulateFormSpotSelect(grindEl.formSpotSearch?.value||"",previous);if(selected&&selected!==previous)grindRenderLootInputs(grindSpotById(selected),{})}
function grindFormatSilver(value){const n=Number(value)||0,abs=Math.abs(n),sign=n<0?"-":"";if(abs>=1e12)return`${sign}${(abs/1e12).toFixed(abs>=10e12?1:2)}T`;if(abs>=1e9)return`${sign}${(abs/1e9).toFixed(abs>=10e9?1:2)}B`;if(abs>=1e6)return`${sign}${(abs/1e6).toFixed(abs>=10e6?1:2)}M`;return`${sign}${Math.round(abs).toLocaleString()}`}
function grindFormatCount(value){return Math.max(0,Math.round(Number(value)||0)).toLocaleString()}
function grindFormatHours(minutes){const total=Math.round(Number(minutes)||0);if(total<=0)return"0 hours";const h=Math.floor(total/60),m=total%60;if(h&&m)return`${h}h ${m}m`;if(h)return`${h}h`;return`${m}m`}
function grindDateLabel(value){const date=new Date(value);if(Number.isNaN(date.getTime()))return"Unknown date";const elapsed=Date.now()-date.getTime(),future=elapsed<0,abs=Math.abs(elapsed),minute=60000,hour=60*minute,day=24*hour,month=30*day,year=365*day;let amount,unit;if(abs<minute)return future?"Soon":"Just now";if(abs<hour){amount=Math.max(1,Math.round(abs/minute));unit=amount===1?"minute":"minutes"}else if(abs<day){amount=Math.max(1,Math.round(abs/hour));unit=amount===1?"hour":"hours"}else if(abs<month){amount=Math.max(1,Math.round(abs/day));unit=amount===1?"day":"days"}else if(abs<year){amount=Math.max(1,Math.round(abs/month));unit=amount===1?"month":"months"}else{amount=Math.max(1,Math.round(abs/year));unit=amount===1?"year":"years"}return future?`in ${amount} ${unit}`:`${amount} ${unit} ago`}
function grindSessionDateValue(session){const date=new Date(session.date||0);return Number.isNaN(date.getTime())?0:date.getTime()}
function grindRangeStart(){if(grindState.range==="all")return 0;const days=Number(grindState.range);if(!Number.isFinite(days))return 0;const start=new Date();start.setHours(0,0,0,0);start.setDate(start.getDate()-days+1);return start.getTime()}
function grindFilteredSessions(){const start=grindRangeStart();return grindSessions().filter(session=>!start||grindSessionDateValue(session)>=start)}
function grindSortSpots(a,b){return String(a.name).localeCompare(String(b.name))}
function grindPopulateSpotSelects(){const spots=GRIND_SPOTS.slice().sort(grindSortSpots);if(!grindSpotById(grindState.selectedSpotId)&&spots[0])grindState.selectedSpotId=String(spots[0].id);grindPopulateFormSpotSelect(grindEl.formSpotSearch?.value||"",grindState.selectedSpotId)}
function grindGroupSessions(sessions){const map=new Map();sessions.forEach(session=>{const spot=grindSpotById(session.spotId);let id=String(spot?.id||session.spotId),name=spot?.name||"Unknown spot",icon=spot?.icon,spotId=id;if(grindState.mode==="class"){id=String(session.className||"Unspecified class").trim()||"Unspecified class";name=id;spotId="";}else if(grindState.mode==="overtime"){id=session.date||"Unknown date";name=grindDateLabel(session.date);spotId="";}if(!map.has(id))map.set(id,{id,name,icon,spotId,silver:0,minutes:0,count:0});const item=map.get(id);item.silver+=Number(session.silver)||0;item.minutes+=Number(session.minutes)||0;item.count+=1;if(!item.icon&&icon)item.icon=icon});return [...map.values()].map((item,index)=>({...item,color:grindColors[index%grindColors.length],hourly:item.minutes?item.silver/(item.minutes/60):0})).sort((a,b)=>b.silver-a.silver||b.minutes-a.minutes||a.name.localeCompare(b.name))}
function grindTopSpotGroups(sessions){const previous=grindState.mode;grindState.mode="spot";const groups=grindGroupSessions(sessions);grindState.mode=previous;return groups}
function grindEmpty(message){return`<div class="grindEmpty">${escapeHtml(message)}</div>`}
function grindRenderSummary(sessions){const silver=sessions.reduce((sum,s)=>sum+(Number(s.silver)||0),0),minutes=sessions.reduce((sum,s)=>sum+(Number(s.minutes)||0),0),average=minutes?silver/(minutes/60):0;if(grindEl.totalSilver)grindEl.totalSilver.textContent=grindFormatSilver(silver);if(grindEl.averageHour)grindEl.averageHour.textContent=grindFormatSilver(average);if(grindEl.totalHours)grindEl.totalHours.textContent=grindFormatHours(minutes);if(grindEl.recentSummary)grindEl.recentSummary.textContent=`${sessions.length} session${sessions.length===1?"":"s"}`}
function grindRenderHours(groups){const total=groups.reduce((sum,g)=>sum+g.minutes,0);if(grindEl.hoursSummary)grindEl.hoursSummary.textContent=total?`${groups.length} group${groups.length===1?"":"s"}`:"No sessions yet";if(!grindEl.hoursLegend||!grindEl.hoursDonut)return;if(!groups.length){grindEl.hoursLegend.innerHTML=grindEmpty("Add a session, review the loot counts, then save it to build this chart.");grindEl.hoursDonut.style.background="conic-gradient(var(--a1) 0 0)";grindEl.hoursDonut.dataset.label="0 hours";return;}let cursor=0;const stops=groups.slice(0,9).map((group,index)=>{const start=cursor,end=cursor+(group.minutes/total)*100;cursor=end;return`${group.color} ${start}% ${end}%`});if(cursor<100)stops.push(`color-mix(in srgb,var(--surface2) 80%,#000) ${cursor}% 100%`);grindEl.hoursDonut.style.background=`conic-gradient(${stops.join(",")})`;grindEl.hoursDonut.dataset.label=grindFormatHours(total);grindEl.hoursLegend.innerHTML=groups.slice(0,9).map(group=>`<div class="grindLegendItem" style="--row-color:${group.color}"><span class="grindLegendDot"></span><span>${grindGroupLabelHtml(group.name)}</span><b>${grindFormatHours(group.minutes)}</b></div>`).join("")}
function grindRenderBars(groups,target,metric){if(!target)return;const sorted=groups.slice().sort((a,b)=>metric==="hourly"?b.hourly-a.hourly:b.silver-a.silver).slice(0,10);const max=Math.max(1,...sorted.map(group=>metric==="hourly"?group.hourly:group.silver));target.innerHTML=sorted.length?sorted.map(group=>{const value=metric==="hourly"?group.hourly:group.silver,width=Math.max(4,Math.round(value/max*100));const clickable=group.spotId?`data-grind-spot-open="${escapeHtml(group.spotId)}"`:"";return`<button class="grindBarRow" ${clickable} type="button" style="--bar-color:${group.color};--bar-width:${width}%"><strong>${grindGroupLabelHtml(group.name)}</strong><span class="grindBarTrack"><i></i></span><em>${grindFormatSilver(value)}</em></button>`}).join(""):grindEmpty("No session data yet.");}
function grindSessionMetaBits(session,{includeClass=true}={}){const bits=[];if(includeClass&&session.className)bits.push(session.className);bits.push(grindDateLabel(session.date));bits.push(`${grindSessionHours(session).toFixed(2)}h`);if(session.agris)bits.push("Agris");if(Number(session.dropRate)>0)bits.push(`${Number(session.dropRate)}% drop rate`);return bits}
function grindSessionDetailMeta(session){return grindSessionMetaBits(session,{includeClass:false}).join(" - ")}
function grindEditingSessionId(){return String(grindEl.id?.value||"")}
function grindSessionEditingClass(sessionId){return String(sessionId)===grindEditingSessionId()?" editing":""}
function grindRefreshEditingIndicators(){const editingId=grindEditingSessionId();document.querySelectorAll("[data-grind-session-id]").forEach(row=>{const active=editingId&&String(row.dataset.grindSessionId)===editingId;row.classList.toggle("editing",!!active);if(active)row.setAttribute("aria-current","true");else row.removeAttribute("aria-current")})}
function grindSessionPricingIncomplete(session){return !!session?.pricingIncomplete||(session?.loot||[]).some(item=>String(item.priceSource||"")==="missing")}
function grindPricingWarningHtml(session){return grindSessionPricingIncomplete(session)?'<span class="grindPricingWarning" title="One or more loot prices were unavailable when this session was saved." aria-label="Incomplete market pricing">!</span>':""}
function grindRenderRecent(sessions){if(!grindEl.recent)return;const recent=sessions.slice().sort((a,b)=>grindSessionDateValue(b)-grindSessionDateValue(a)).slice(0,12);grindEl.recent.innerHTML=recent.length?recent.map(session=>{const spot=grindSpotById(session.spotId),hours=grindSessionHours(session),className=String(session.className||"Unspecified").trim()||"Unspecified",meta=grindSessionDetailMeta(session),editing=grindSessionEditingClass(session.id);return`<article class="grindSessionRow${editing}" data-grind-session-id="${escapeHtml(session.id)}" data-grind-session-open="${escapeHtml(session.spotId)}" ${editing?'aria-current="true"':""}><img src="${escapeHtml(spot?.icon||"")}" alt=""><div><strong>${escapeHtml(spot?.name||"Unknown spot")}</strong><span class="grindRecentMeta">${grindClassBadgeHtml(className)}<span>${escapeHtml(meta)}</span></span>${grindSessionBuffStrip(session,10,"")}</div><div class="grindSessionValue"><b>${grindFormatSilver(session.silver)}${grindPricingWarningHtml(session)}</b><small>${grindFormatSilver(hours?session.silver/hours:0)}/h</small></div><div style="display:flex;gap:6px"><button class="grindIconButton" data-grind-edit="${escapeHtml(session.id)}" type="button" title="${editing?"Editing session":"Edit session"}">E</button><button class="grindIconButton danger" data-grind-delete="${escapeHtml(session.id)}" type="button" title="Delete">X</button></div></article>`}).join(""):grindEmpty("No sessions saved yet.");}
function grindLootColor(drop){if(drop.isTrash)return"#fbbf24";const grade=Number(drop.grade);return grade>=4?"#f97316":grade===3?"#a78bfa":grade===2?"#60a5fa":grade===1?"#34d399":"#cbd5e1"}
function grindRenderLootInputs(spot=grindSpotById(grindState.selectedSpotId),counts={}){if(!grindEl.lootInputs)return;const drops=Array.isArray(spot?.drops)?spot.drops:[];grindEl.lootInputs.innerHTML=drops.length?drops.map(drop=>`<label class="grindLootInputCard" style="--loot-color:${grindLootColor(drop)}"><img src="${escapeHtml(drop.icon||"")}" alt=""><span><strong>${escapeHtml(drop.name)}</strong>${grindDropPriceSmall(drop)}<input type="number" min="0" step="1" value="${Number(counts[String(drop.id)]||0)}" data-grind-loot-count="${escapeHtml(drop.id)}" data-grind-loot-name="${escapeHtml(drop.name)}" data-grind-loot-icon="${escapeHtml(drop.icon||"")}" data-grind-loot-trash="${drop.isTrash?"1":"0"}"></span></label>`).join(""):grindEmpty("No drop table available for this spot yet.")}
function grindCollectLoot(){return [...(grindEl.lootInputs?.querySelectorAll("[data-grind-loot-count]")||[])].map(input=>({id:String(input.dataset.grindLootCount),name:input.dataset.grindLootName||"",icon:input.dataset.grindLootIcon||"",isTrash:input.dataset.grindLootTrash==="1",count:Math.max(0,Math.round(Number(input.value)||0))})).filter(item=>item.count>0)}
function grindPriceLootItems(loot,spot=grindSpotById(grindState.selectedSpotId),previousLoot=[]){const drops=new Map((spot?.drops||[]).map(drop=>[String(drop.id),drop])),previous=new Map((previousLoot||[]).map(item=>[String(item.id),item])),capturedAt=new Date().toISOString();return(loot||[]).map(item=>{const id=String(item.id),drop=drops.get(id)||item,old=previous.get(id),hasHistoricalPrice=old&&String(old.priceSource||"")!=="missing"&&Number.isFinite(Number(old.price)),record=hasHistoricalPrice?{price:Number(old.price),source:old.priceSource||"historical",region:old.priceRegion||grindState.marketRegion,capturedUtc:old.priceCapturedAt||capturedAt}:grindDropHasNoValue(drop)?{price:0,source:"no-value",region:"none",capturedUtc:capturedAt}:grindPriceRecordForDrop(drop),price=Math.max(0,Number(record?.price)||0),count=Math.max(0,Number(item.count)||0);return{...item,price,value:price*count,priceSource:record?.source||"missing",priceRegion:record?.region==="fixed"?"fixed":record?.region||grindState.marketRegion,priceCapturedAt:record?.capturedUtc||capturedAt}})}
function grindLootValue(loot){return(loot||[]).reduce((sum,item)=>sum+(Number(item.value)||Number(item.price||0)*Number(item.count||0)||0),0)}
function grindSetAgris(value){const enabled=!!value;if(grindEl.agris)grindEl.agris.checked=enabled;document.querySelectorAll("[data-grind-agris]").forEach(button=>button.classList.toggle("active",(button.dataset.grindAgris==="on")===enabled))}
function grindCurrentAgris(){return !!grindEl.agris?.checked}
function initializeGrindTracker(){
  const view=document.getElementById("grindTrackerView");
  if(!view)return;
  grindState.marketRegion="eu";
  persistSetting("grindTrackerMarketRegion","eu");
  grindPopulateSpotSelects();
  grindPopulateClassSelect();
  grindBindClassPicker();
  grindRenderBuffPicker();
  grindUpdateMarketRegionButtons();
  grindSchedulePriceRefresh();
  grindLoadSessions();
  if(!grindState.powerMode)grindState.powerMode=readSetting("grindTrackerPowerMode","recommended");
  if(!grindState.screen)grindState.screen="summary";
  if(typeof grindState.pickerSearch!=="string")grindState.pickerSearch="";
  if(!grindState.initialized){
    grindState.initialized=true;
    let formSearchTimer=null,pickerSearchTimer=null;
    document.querySelectorAll("[data-grind-mode]").forEach(button=>button.addEventListener("click",()=>{
      grindState.mode=button.dataset.grindMode||"spot";
      persistSetting("grindTrackerMode",grindState.mode);
      grindRender();
    }));
    document.querySelectorAll("[data-grind-range]").forEach(button=>button.addEventListener("click",()=>{
      grindState.range=button.dataset.grindRange||"all";
      persistSetting("grindTrackerRange",grindState.range);
      grindRender();
    }));
    document.querySelectorAll("[data-grind-market-region]").forEach(button=>button.addEventListener("click",()=>{
      grindState.marketRegion="eu";
      persistSetting("grindTrackerMarketRegion","eu");
      grindUpdateMarketRegionButtons();

      grindRender();
      grindRefreshMarketPrices({force:false,silent:false});
    }));
    document.querySelectorAll("[data-grind-agris]").forEach(button=>button.addEventListener("click",()=>{
      grindSetAgris(button.dataset.grindAgris==="on");
      grindPersistCurrentSessionDefaults();
    }));
    document.querySelectorAll("[data-grind-power-mode]").forEach(button=>button.addEventListener("click",()=>{
      grindState.powerMode=button.dataset.grindPowerMode||"recommended";
      persistSetting("grindTrackerPowerMode",grindState.powerMode);
      grindRenderSpotPicker();
    }));
    grindEl.formSpotSearch?.addEventListener("input",()=>{
      clearTimeout(formSearchTimer);
      formSearchTimer=setTimeout(grindFilterFormSpotSelect,100);
    });
    grindEl.formSpot?.addEventListener("change",()=>grindSelectSpotForSession(grindEl.formSpot.value,{openPicker:false,review:false}));
    grindEl.add?.addEventListener("click",grindOpenSpotPicker);
    document.getElementById("grindBackSummary")?.addEventListener("click",()=>{
      grindStopDraft();
      grindSetScreen("summary");
      grindSetReviewReady(false);
      grindRender();
    });
    document.getElementById("grindSpotPickerClose")?.addEventListener("click",()=>grindCloseSpotPicker());
    document.getElementById("grindSpotPickerSearch")?.addEventListener("input",event=>{
      clearTimeout(pickerSearchTimer);
      pickerSearchTimer=setTimeout(()=>{grindState.pickerSearch=event.target.value||"";grindRenderSpotPicker()},100);
    });
    document.getElementById("grindSpotPicker")?.addEventListener("click",event=>{if(event.target.id==="grindSpotPicker")grindCloseSpotPicker()});
    grindEl.start?.addEventListener("click",grindStartDraft);
    grindBindImageDrop(view);
    grindEl.cancel?.addEventListener("click",()=>grindResetForm(grindState.selectedSpotId));
    [grindEl.className,grindEl.dropRate,grindEl.hours,grindEl.minutes].forEach(input=>input?.addEventListener("change",grindPersistCurrentSessionDefaults));
    grindEl.buffs?.addEventListener("click",event=>{
      const button=event.target.closest("[data-grind-buff]");
      if(!button)return;
      button.classList.toggle("active");
      button.setAttribute("aria-pressed",button.classList.contains("active")?"true":"false");
      grindPersistCurrentSessionDefaults();
    });
    grindEl.form?.addEventListener("submit",grindSaveForm);
    view.addEventListener("click",event=>{
      const picker=event.target.closest("[data-grind-picker-spot]");
      if(picker){grindSelectSpotForSession(picker.dataset.grindPickerSpot,{openPicker:false,review:false});return}
      const edit=event.target.closest("[data-grind-edit]");
      if(edit){grindEditSession(edit.dataset.grindEdit);return}
      const page=event.target.closest("[data-grind-session-page]");
      if(page){grindState.spotSessionPage=Math.max(0,Number(page.dataset.grindSessionPage)||0);grindRender();return}
      const sessionOpen=event.target.closest("[data-grind-session-open]");
      if(sessionOpen){grindSelectSpotForSession(sessionOpen.dataset.grindSessionOpen,{openPicker:false,review:false});return}
      const spotButton=event.target.closest("[data-grind-spot-open]");
      if(spotButton)grindSelectSpotForSession(spotButton.dataset.grindSpotOpen,{openPicker:false,review:false});
    });
  }
  grindCloseSpotPicker(false);
  grindSetScreen("summary");
  grindSetReviewReady(false);
  grindResetForm(grindState.selectedSpotId);
  grindRender();
}
document.getElementById("grindTrackerView")?.addEventListener("click",async event=>{
  const button=event.target.closest("[data-grind-delete]");
  if(!button)return;
  event.preventDefault();
  event.stopImmediatePropagation();
  const session=grindSessions().find(item=>item.id===button.dataset.grindDelete);
  if(!session)return;
  const spot=grindSpotById(session.spotId);
  if(!await appConfirm(`Delete the saved session at ${spot?.name||"this spot"}?`,{title:"Delete grind session",acceptLabel:"Delete session"}))return;
  try{
    await saveGrindSessions(grindSessions().filter(item=>item.id!==session.id));
    grindRender();
    NotificationService.ShowWarning("Grind session deleted.","Grind Tracker");
  }catch(error){
    NotificationService.ShowError(error.message||"Could not delete the session.","Grind Tracker");
  }
},true);

function grindShellNode(){return document.querySelector("#grindTrackerView .grindShell")}
function grindNormalizeName(value){return String(value||"").toLowerCase().replace(/[\[\]'()]/g,"").replace(/[^a-z0-9]+/g," ").trim()}
function grindTrashDrop(spot){const drops=Array.isArray(spot?.drops)?spot.drops:[],trashId=String(spot?.trashId||""),primary=String(spot?.primaryTrash||"").trim().toLowerCase();return drops.find(drop=>trashId&&String(drop.id)===trashId)||drops.find(drop=>primary&&String(drop.name||"").trim().toLowerCase()===primary)||drops.find(drop=>drop.isTrash)||drops[0]||null}
function grindSpotTrashCount(session,spot){const loot=Array.isArray(session?.loot)?session.loot:[],trash=grindTrashDrop(spot),trashId=String(trash?.id||spot?.trashId||"");if(trashId)return loot.reduce((sum,item)=>sum+(String(item.id)===trashId?Math.max(0,Number(item.count)||0):0),0);return loot.reduce((sum,item)=>sum+(item.isTrash?Math.max(0,Number(item.count)||0):0),0)}
const grindCcIconMap={stiffness:"Assets/GrindTracker/cc/stiffness.png",stun:"Assets/GrindTracker/cc/stun.png",freeze:"Assets/GrindTracker/cc/freeze.png",knockdown:"Assets/GrindTracker/cc/knockdown.png",bound:"Assets/GrindTracker/cc/bound.png",knockback:"Assets/GrindTracker/cc/knockback.png",float:"Assets/GrindTracker/cc/float.png"};
const grindCcLabels={stiffness:"Stiffness",stun:"Stun",freeze:"Freeze",knockdown:"Knockdown",bound:"Bound",knockback:"Knockback",float:"Float"};
const grindCcOrder=["stiffness","stun","freeze","knockdown","bound","knockback","float"];
const grindSpotCcOverrides={"dark energy floodlands":["knockdown","bound"],"zephyros castle":["knockdown","bound"],"tenebraum castle":["knockdown","bound"],"orbita castle":["knockdown","bound"],"aetherion castle":["stun","stiffness","freeze"],"nymphamar castle":["stun","stiffness","freeze"],"stars end":["stiffness","stun","freeze"],"star s end":["stiffness","stun","freeze"],"sycraia abyssal ruins lower":["knockback","float"],"old sycraia lower":["knockdown","bound"],"hystria ruins":["knockback","float"],"aakman temple":["knockdown","bound"],"dehkia hystria ruins":["knockback","float"],"dehkia aakman temple":["knockdown","bound"],"dehkia cyclops land":["knockback","float"],"dehkia cadry ruins":["stun","stiffness","freeze"],"dehkia crescent shrine":["knockdown","bound"],"dehkia ash forest":["knockdown","bound"],"dehkia 2 ash forest":["knockdown","bound"],"dehkia tunkuta":["knockdown","bound"],"dehkia thornwood forest":["knockback","knockdown"],"dehkia olun s valley":["knockdown","bound"],"dehkia 2 olun s valley":["knockdown","bound"],"gyfin rhasia underground":["knockdown","bound"],"olun s valley":["knockdown","bound"],"crypt of resting thoughts":["stun","stiffness","freeze"],"orcs camp":["stun","stiffness","freeze"],"orc camp":["stun","stiffness","freeze"],"bloody monastery":["stun","stiffness","freeze"],"biraghi den":["stun","stiffness","freeze"],"swamp fogan habitat":["stun","stiffness","freeze"],"swamp naga habitat":["stun","stiffness","freeze"],"saunil camp":["knockdown","bound"],"centaurus herd":["knockdown","bound"],"cadry ruins":["stun","stiffness","freeze"],"crescent shrine":["knockdown","bound"],"bashim base":[],"desert naga temple":[],"tshira ruins":["knockdown","bound"],"roud sulfur mine":["knockback","float"],"pila ku jail":["knockdown","bound"],"basilisk den":["knockback","float"],"traitor s graveyard":[],"gahaz bandit s lair":["stun","stiffness","freeze"],"zephyros dark energy floodlands":["knockdown","bound"],"orbita dark energy floodlands":["knockdown","bound"],"great red sea dark energy floodlands":["knockdown","bound"]};
const grindMaxCapOverrides={"stars end":[1950,800],"star s end":[1950,800],"zephyros castle":[2010,800],"sycraia abyssal ruins lower":[1935,800],"old sycraia lower":[800,290],"tenebraum castle":[1920,760],"dark energy floodlands":[1880,760],"orbita castle":[1800,740],"aetherion castle":[1595,615],"nymphamar castle":[1690,720],"elvia orzekea":[1595,700],"dehkia gyfin rhasia temple upper":[1680,715],"dehkia mirumok ruins":[1595,615],"dehkia 2 ash forest":[1540,540],"dehkia 2 olun s valley":[1490,540],"dokkebi forest":[1445,530],"fortunate golden pig cave":[1490,660],"unlucky golden pig cave":[1540,670],"yzrahid highlands":[1180,460],"quint hill":[1295,440],"hexe sanctuary":[1130,430],"dehkia thornwood forest":[1180,440],"dehkia cadry ruins":[1395,440],"dehkia cyclops land":[1180,440],"dehkia crescent shrine":[1350,440],"dehkia ash forest":[1350,440],"city of the dead":[1295,410],"dehkia tunkuta":[1180,440],"dehkia roud sulfur mine":[1130,440],"dehkia hystria ruins":[1130,440],"dehkia aakman temple":[1130,440],"dehkia pila ku jail":[1130,440],"sycraia abyssal ruins upper":[1130,440],"ash forest":[1130,430],"gyfin rhasia underground":[1030,410],"jade starlight forest":[950,370],"honglim base":[950,390],"crypt of resting thoughts":[1130,440],"olun s valley":[1030,410],"primal giant post":[1000,410],"swamp fogan habitat":[803,290],"winter tree fossil 280ap":[856,370],"winter tree fossil 280":[856,370],"orc camp":[856,350],"orcs camp":[856,350],"rhutum outstation":[835,380],"altar imp habitat":[753,280],"swamp naga habitat":[803,290],"saunil camp":[813,330],"biraghi den":[753,280],"murrowak s labyrinth":[856,370],"bloody monastery":[856,350],"tunkuta":[825,340],"sherekhan night":[480,170],"abandoned monastery":[856,370],"gyfin rhasia temple":[825,280],"crescent shrine":[245,125],"blood wolf settlement":[312,150],"thornwood forest":[756,280],"waragon nest":[250,160],"padix island":[825,340],"kratuga ancient ruins":[756,250],"vessel of inquisition pillars":[800,280],"castle ruins":[774,280],"polly s forest":[255,180],"mirumok ruins":[560,220],"fadus habitat":[280,140],"sherekhan day":[365,170],"vessel of inquisition":[800,280],"tooth fairy forest":[410,220],"centaurus herd":[312,145],"cadry ruins":[245,125],"gahaz bandit s lair":[245,140],"aakman temple":[600,250],"hystria ruins":[756,250],"protty cave":[280,145],"desert naga temple":[213,140],"bashim base":[213,140],"tshira ruins":[245,125],"roud sulfur mine":[365,180],"pila ku jail":[365,180],"basilisk den":[320,160],"traitor s graveyard":[255,250],"zephyros dark energy floodlands":[1880,760],"orbita dark energy floodlands":[1880,760],"great red sea dark energy floodlands":[1880,760]};
function grindMonsterMeta(spot){const type=String(spot?.type||"normal").toLowerCase();const map={human:["Human","monster-human.png"],demi:["Demihuman","monster-demi.png"],kama:["Kama","monster-kama.png"],edania:["Edania","monster-edania.png"],normal:["Normal","monster-normal.png"]};const key=type in map?type:"normal",data=map[key];return{type:key,label:data[0],icon:`Assets/GrindTracker/icons-clean/${data[1]}`}}
function grindSpotCcs(spot){const key=grindNormalizeName(spot?.name);if(Object.prototype.hasOwnProperty.call(grindSpotCcOverrides,key))return grindSpotCcOverrides[key];const type=String(spot?.type||"normal").toLowerCase();let effects=type==="human"?["stun","stiffness","freeze"]:type==="kama"?["knockdown","bound"]:type==="demi"?["knockdown","bound"]:type==="edania"?["knockdown","bound"]:["knockback","float"];return grindCcOrder.filter(item=>effects.includes(item))}
function grindRecommendedPower(spot){return{ap:Number(spot?.ap)||0,dp:Number(spot?.dp)||0,label:"Recommended"}}
function grindMaxPower(spot){const key=grindNormalizeName(spot?.name),override=grindMaxCapOverrides[key];if(override)return{ap:override[0],dp:override[1],label:"Max"};const ap=Number(spot?.ap)||0,dp=Number(spot?.dp)||0;return{ap:ap?Math.round(ap*3.2):0,dp:dp?Math.round(dp*1.15):0,label:"Max est."}}
function grindPowerForSpot(spot){return grindState.powerMode==="max"?grindMaxPower(spot):grindRecommendedPower(spot)}
function grindPowerText(spot){const power=grindPowerForSpot(spot);return`${power.ap?power.ap.toLocaleString():"-"} AP | ${power.dp?power.dp.toLocaleString():"-"} DP`}
function grindSetScreen(screen){grindState.screen=screen==="spot"?"spot":"summary";const shell=grindShellNode();if(shell)shell.dataset.grindScreen=grindState.screen}
function grindSetReviewReady(value){grindDraft.reviewReady=!!value;const shell=grindShellNode();if(shell)shell.classList.toggle("grindReviewReady",grindDraft.reviewReady)}
function grindOpenSpotPicker(){
  grindPickerReturnFocus=document.activeElement instanceof HTMLElement?document.activeElement:null;
  grindState.pickerSearch="";
  const search=document.getElementById("grindSpotPickerSearch"),picker=document.getElementById("grindSpotPicker");
  if(search)search.value="";
  grindRenderSpotPicker();
  if(picker)picker.hidden=false;
  setTimeout(()=>search?.focus(),30);
}
function grindCloseSpotPicker(restoreFocus=true){
  const picker=document.getElementById("grindSpotPicker");
  if(picker)picker.hidden=true;
  if(restoreFocus&&grindPickerReturnFocus?.isConnected)setTimeout(()=>grindPickerReturnFocus.focus(),0);
  grindPickerReturnFocus=null;
}
document.getElementById("grindSpotPicker")?.addEventListener("keydown",event=>{
  const picker=event.currentTarget;
  if(event.key==="Escape"){event.preventDefault();grindCloseSpotPicker();return}
  if(event.key!=="Tab")return;
  const focusable=[...picker.querySelectorAll('button:not([disabled]),input:not([disabled]),select:not([disabled]),[tabindex]:not([tabindex="-1"])')].filter(element=>element.offsetParent!==null);
  if(!focusable.length)return;
  const first=focusable[0],last=focusable[focusable.length-1];
  if(event.shiftKey&&document.activeElement===first){event.preventDefault();last.focus()}
  else if(!event.shiftKey&&document.activeElement===last){event.preventDefault();first.focus()}
});
function grindSelectSpotForSession(spotId,{openPicker=false,review=false}={}){const id=String(spotId||grindState.selectedSpotId||GRIND_SPOTS[0]?.id||"");if(!id)return;grindCloseSpotPicker();grindState.selectedSpotId=id;grindState.spotSessionPage=0;persistSetting("grindTrackerSelectedSpot",id);if(grindEl.formSpotSearch)grindEl.formSpotSearch.value="";grindPopulateFormSpotSelect("",id);grindSetScreen("spot");grindSetReviewReady(!!review);grindResetForm(id,false);grindRender();grindEnsureMarketPricesForSpot(id);if(openPicker)grindOpenSpotPicker()}
function grindRenderSpotPicker(){document.querySelectorAll("[data-grind-power-mode]").forEach(button=>button.classList.toggle("active",(button.dataset.grindPowerMode||"recommended")===grindState.powerMode));const header=document.getElementById("grindPowerHeader");if(header)header.textContent=grindState.powerMode==="max"?"Max AP / DP":"Recommended AP / DP";const list=document.getElementById("grindSpotPickerList");if(!list)return;const query=String(grindState.pickerSearch||"").trim().toLowerCase();const spots=GRIND_SPOTS.filter(spot=>{if(!query)return true;return`${spot.name} ${spot.zone} ${spot.primaryTrash}`.toLowerCase().includes(query)}).sort((a,b)=>{const aPower=grindPowerForSpot(a),bPower=grindPowerForSpot(b);const apDiff=(bPower.ap||0)-(aPower.ap||0);if(apDiff)return apDiff;const dpDiff=(bPower.dp||0)-(aPower.dp||0);if(dpDiff)return dpDiff;return String(a.name).localeCompare(String(b.name))});list.innerHTML=spots.length?spots.map(spot=>{const trash=grindTrashDrop(spot),monster=grindMonsterMeta(spot),ccs=grindSpotCcs(spot);const ccHtml=ccs.length?ccs.map(cc=>`<img class="grindCcIcon" src="${escapeHtml(grindCcIconMap[cc])}" alt="${escapeHtml(grindCcLabels[cc])}" title="${escapeHtml(grindCcLabels[cc])}">`).join(""):`<span class="grindNoCc">-</span>`;return`<button class="grindPickerRow" data-grind-picker-spot="${escapeHtml(spot.id)}" type="button"><span class="grindPickerName"><span class="grindPickerIcons"><img class="grindPickerLoot" src="${escapeHtml(trash?.icon||spot.icon||"")}" alt=""><img class="grindMonsterBadge ${escapeHtml(monster.type)}" src="${escapeHtml(monster.icon)}" alt="${escapeHtml(monster.label)}" title="${escapeHtml(monster.label)}"></span><span><strong>${escapeHtml(spot.name)}</strong><small>${escapeHtml(spot.zone||"Unknown region")} - ${escapeHtml(trash?.name||spot.primaryTrash||"Trash loot")}</small></span></span><span class="grindPickerCc">${ccHtml}</span><span class="grindCapText">${escapeHtml(grindPowerText(spot))}</span></button>`}).join(""):grindEmpty("No grind spots match that search.")}
function grindRenderSpotDetail(sessions){
	if(!grindEl.spotDetail)return;
	const spot=grindSpotById(grindState.selectedSpotId);
	if(!spot){grindEl.spotDetail.innerHTML=grindEmpty("No grind-zone data found.");return;}
	const spotSessions=sessions.filter(s=>String(s.spotId)===String(spot.id)).sort((a,b)=>grindSessionDateValue(b)-grindSessionDateValue(a));
	const silver=spotSessions.reduce((sum,s)=>sum+(Number(s.silver)||0),0),minutes=spotSessions.reduce((sum,s)=>sum+(Number(s.minutes)||0),0),trash=grindTrashDrop(spot);
	const hours=minutes/60,silverPerHour=hours?silver/hours:0,totalTrash=spotSessions.reduce((sum,s)=>sum+grindSpotTrashCount(s,spot),0),trashPerHour=hours?totalTrash/hours:0;
	const kpis=`<div class="grindSpotKpiGrid"><div class="grindSpotKpiCard" data-tone="silver"><small>Total Silver Earned</small><strong>${grindFormatSilver(silver)}</strong><span class="grindSpotKpiIcon" aria-hidden="true">$</span></div><div class="grindSpotKpiCard" data-tone="hourly"><small>Average Silver an Hour</small><strong>${grindFormatSilver(silverPerHour)}</strong><span class="grindSpotKpiIcon" aria-hidden="true">/h</span></div><div class="grindSpotKpiCard" data-tone="trash"><small>Trash an Hour</small><strong>${grindFormatCount(trashPerHour)}</strong><span class="grindSpotKpiIcon" aria-hidden="true">T</span></div></div>`;
	const loot=(spot.drops||[]).map(drop=>`<div class="grindLootCard" style="--loot-color:${grindLootColor(drop)}"><img src="${escapeHtml(drop.icon||"")}" alt=""><div><strong>${escapeHtml(drop.name)}</strong>${grindDropPriceLine(drop)}</div></div>`).join("");
	const pageSize=25,pageCount=Math.max(1,Math.ceil(spotSessions.length/pageSize));
	grindState.spotSessionPage=Math.min(Math.max(0,grindState.spotSessionPage||0),pageCount-1);
	const pageStart=grindState.spotSessionPage*pageSize,pagedSessions=spotSessions.slice(pageStart,pageStart+pageSize);
	const sessionRows=pagedSessions.map(session=>{
		const className=String(session.className||"Unspecified").trim()||"Unspecified";
		const editing=grindSessionEditingClass(session.id);
		return`<div class="grindSpotSessionLine${editing}" data-grind-session-id="${escapeHtml(session.id)}" ${editing?'aria-current="true"':""}><span class="grindSessionDateCell">${grindDateLabel(session.date)}</span><span class="grindSessionTimeCell">${escapeHtml(grindFormatHours(Number(session.minutes)||0))}</span><span class="grindSessionAgrisCell ${session.agris?"on":"off"}">${escapeHtml(session.agris?"On":"Off")}</span><span class="grindSessionBuffCell">${grindSessionBuffStrip(session,8,"")||'<span class="grindNoBuffs">-</span>'}</span><strong class="grindSessionClassCell" title="${escapeHtml(className)}">${grindClassBadgeHtml(className)}</strong><b class="grindSessionTotalCell">${grindFormatSilver(session.silver)}${grindPricingWarningHtml(session)}</b><span class="grindSessionActions"><button class="grindIconButton" data-grind-edit="${escapeHtml(session.id)}" type="button" title="${editing?"Editing session":"Edit session"}">E</button><button class="grindIconButton danger" data-grind-delete="${escapeHtml(session.id)}" type="button" title="Delete">X</button></span></div>`;
	}).join("");
	const pager=pageCount>1?`<div class="grindSessionPager"><button type="button" data-grind-session-page="${grindState.spotSessionPage-1}" ${grindState.spotSessionPage===0?"disabled":""}>Previous</button><span>Page ${grindState.spotSessionPage+1} of ${pageCount}</span><button type="button" data-grind-session-page="${grindState.spotSessionPage+1}" ${grindState.spotSessionPage>=pageCount-1?"disabled":""}>Next</button></div>`:"";
	grindEl.spotDetail.innerHTML=`<div class="grindSpotTop"><img src="${escapeHtml(spot.icon||trash?.icon||"")}" alt=""><div><h2>${escapeHtml(spot.name)}</h2><p>${escapeHtml(spot.zone||"Unknown region")} - ${escapeHtml(trash?.name||spot.primaryTrash||"Trash loot")} - ${spot.players||1} player${String(spot.players||"1")==="1"?"":"s"}</p></div><div class="grindSpotStats"><span>${spot.ap||"-"} AP</span><span>${spot.dp||"-"} DP</span><span>${spotSessions.length} sessions</span><span>${grindFormatHours(minutes)}</span></div></div>${kpis}<div class="grindLootGrid">${loot||grindEmpty("No drop table available for this spot.")}</div><div class="grindSpotSessions"><h3>Saved sessions at this spot</h3>${spotSessions.length?`<div class="grindSpotSessionTable"><div class="grindSpotSessionHeader"><span>Time Ago</span><span>Time</span><span>Agris</span><span>Buffs</span><span>Class</span><span>Total</span><span></span></div>${sessionRows}</div>${pager}`:grindEmpty("No sessions saved for this spot yet.")}</div>`;
}
function grindRender(){const sessions=grindFilteredSessions();document.querySelectorAll("[data-grind-mode]").forEach(button=>button.classList.toggle("active",button.dataset.grindMode===grindState.mode));document.querySelectorAll("[data-grind-range]").forEach(button=>button.classList.toggle("active",button.dataset.grindRange===grindState.range));if(grindState.screen==="summary"){const groups=grindGroupSessions(sessions),spotGroups=grindTopSpotGroups(sessions);grindRenderSummary(sessions);grindRenderHours(groups);grindRenderBars(groups,grindEl.silverBars,"silver");grindRenderBars(groups,grindEl.hourlyBars,"hourly");grindRenderRecent(sessions);if(grindEl.silverSummary)grindEl.silverSummary.textContent=grindState.mode==="spot"?"By spot":grindState.mode==="class"?"By class":"By day";if(grindEl.hourlySummary)grindEl.hourlySummary.textContent=spotGroups.length?`Best: ${spotGroups.slice().sort((a,b)=>b.hourly-a.hourly)[0].name}`:"Best average"}else if(grindState.screen==="spot")grindRenderSpotDetail(sessions);const picker=document.getElementById("grindSpotPicker");if(picker&&!picker.hidden)grindRenderSpotPicker()}
function grindSetImagePreview(result,message){if(!grindEl.imagePreview)return;if(!result?.dataUrl){grindEl.imagePreview.classList.remove("active");grindEl.imagePreview.innerHTML="";return;}const name=result.fileName||"Loot screenshot";grindEl.imagePreview.classList.add("active");grindEl.imagePreview.innerHTML=`<img src="${escapeHtml(result.dataUrl)}" alt="Loot screenshot preview"><span><strong>${escapeHtml(name)}</strong><small>${escapeHtml(message||"Screenshot loaded for review.")}</small></span>`}
function grindScanDropsPayload(){const spot=grindSpotById(grindEl.formSpot?.value||grindState.selectedSpotId)||grindSpotById(grindState.selectedSpotId);return(spot?.drops||[]).map(drop=>({id:String(drop.id),name:drop.name||"",icon:drop.icon||""}))}
function grindApplyImageMatches(matches){let applied=0;(Array.isArray(matches)?matches:[]).forEach(match=>{const id=String(match?.id||"");const count=Math.max(0,Math.round(Number(match?.count)||0));if(!id||count<=0)return;const input=[...(grindEl.lootInputs?.querySelectorAll("[data-grind-loot-count]")||[])].find(node=>String(node.dataset.grindLootCount)===id);if(!input)return;input.value=String(count);applied+=1});return applied}
function grindImageResultMessage(applied,result){const scanCount=Array.isArray(result?.matches)?result.matches.length:0;if(applied)return`Matched ${applied} loot item${applied===1?"":"s"} from the screenshot. Review counts before saving.`;if(scanCount)return"Screenshot scanned, but the matched drops are not in the current form. Check the selected grind zone.";return"Screenshot loaded. No matching drops were found for the selected grind zone, so review counts manually."}
function grindApplyImageScanResult(result){const imageApplied=grindApplyImageMatches(result?.matches);const textApplied=imageApplied?0:grindApplyScreenshotLootText(result?.screenshotText||"");const applied=imageApplied||textApplied;const message=imageApplied?grindImageResultMessage(imageApplied,result):(textApplied?`Matched ${textApplied} loot item${textApplied===1?"":"s"} from screenshot text. Review counts before saving.`:grindImageResultMessage(0,result));grindSetImagePreview(result,message);if(grindEl.draftStatus)grindEl.draftStatus.textContent=message;if(applied)NotificationService.ShowSuccess(message,"Grind Tracker");else NotificationService.ShowInfo(message,"Grind Tracker");return applied}
function grindReadImageFile(file){return new Promise((resolve,reject)=>{if(Number(file?.size)>25*1024*1024){reject(new Error("The image is larger than the 25 MB limit."));return}const reader=new FileReader();reader.onload=()=>resolve(String(reader.result||""));reader.onerror=()=>reject(reader.error||new Error("Could not read image."));reader.readAsDataURL(file)})}
async function grindScanImageFile(file){if(!file||!String(file.type||"").startsWith("image/")){NotificationService.ShowWarning("Drop an image file to scan loot.","Grind Tracker");return;}grindSetScreen("spot");grindSetReviewReady(true);if(grindEl.draftStatus)grindEl.draftStatus.textContent="Scanning dropped screenshot...";const dataUrl=await grindReadImageFile(file);const result=await bridgeCall("scanGrindLootImage",{fileName:file.name||"Loot screenshot",dataUrl,drops:grindScanDropsPayload(),spotId:grindState.selectedSpotId});grindApplyImageScanResult(result)}
function grindBindImageDrop(view){if(grindState.imageDropBound)return;grindState.imageDropBound=true;const dropZone=grindEl.form?.closest(".grindSessionPanel")||grindEl.form||view;const setDrag=active=>{dropZone?.classList.toggle("dragOver",active);document.querySelector(".grindImageActions")?.classList.toggle("dragOver",active)};["dragenter","dragover"].forEach(type=>dropZone?.addEventListener(type,event=>{event.preventDefault();event.dataTransfer.dropEffect="copy";setDrag(true)}));["dragleave","dragend"].forEach(type=>dropZone?.addEventListener(type,()=>setDrag(false)));dropZone?.addEventListener("drop",event=>{event.preventDefault();setDrag(false);const file=[...(event.dataTransfer?.files||[])].find(item=>String(item.type||"").startsWith("image/"));if(file)grindScanImageFile(file).catch(error=>NotificationService.ShowError(error.message||"Could not scan dropped image.","Grind Tracker"))});document.addEventListener("paste",event=>{if(activeAppViewId!=="grindTrackerView")return;const file=[...(event.clipboardData?.files||[])].find(item=>String(item.type||"").startsWith("image/"));if(!file)return;event.preventDefault();grindScanImageFile(file).catch(error=>NotificationService.ShowError(error.message||"Could not scan pasted image.","Grind Tracker"))})}
function grindScreenshotTextNumber(value){const cleaned=String(value||"").replace(/[Oo]/g,"0").replace(/[Il|]/g,"1").replace(/[^\d]/g,"");return cleaned?Math.max(0,Number(cleaned)||0):0}
function grindApplyScreenshotLootText(text){const raw=String(text||"");if(!raw.trim())return 0;let applied=0;const compact=raw.toLowerCase().replace(/[^\p{L}\p{N}]+/gu," ");[...(grindEl.lootInputs?.querySelectorAll("[data-grind-loot-count]")||[])].forEach(input=>{const name=String(input.dataset.grindLootName||""),tokens=name.toLowerCase().replace(/[^\p{L}\p{N}]+/gu," ").trim().split(/\s+/).filter(Boolean);if(!tokens.length)return;const key=tokens.join(" ");const index=compact.indexOf(key);if(index<0)return;const nearby=compact.slice(index+key.length,index+key.length+36);const match=nearby.match(/(\d[\d\s,\.]{0,14})/);const count=match?grindScreenshotTextNumber(match[1]):0;if(count>0){input.value=String(count);applied+=1}});return applied}
function grindStopDraft(){if(grindEl.start)grindEl.start.textContent="Add Image";if(grindEl.draftStatus)grindEl.draftStatus.textContent="Add a loot screenshot to scan, or enter counts manually.";grindSetReviewReady(true)}
async function grindStartDraft(){if(!grindEl.start)return;grindSetScreen("spot");grindSetReviewReady(true);const original=grindEl.start.textContent;grindEl.start.disabled=true;grindEl.start.textContent="Scanning...";if(grindEl.draftStatus)grindEl.draftStatus.textContent="Choose a loot screenshot to scan.";try{const result=await bridgeCall("selectGrindLootImage",{spotId:grindState.selectedSpotId,drops:grindScanDropsPayload()});if(result?.cancelled){if(grindEl.draftStatus)grindEl.draftStatus.textContent="Image import cancelled. Counts can still be entered manually.";return;}grindApplyImageScanResult(result)}catch(error){if(grindEl.draftStatus)grindEl.draftStatus.textContent=error.message||"Could not import image.";NotificationService.ShowError(error.message||"Could not import image.","Grind Tracker")}finally{grindEl.start.disabled=false;grindEl.start.textContent=original||"Add Image"}}
function grindResetForm(spotId=grindState.selectedSpotId,stopDraft=true){if(stopDraft){grindStopDraft();grindSetImagePreview(null)}const spot=grindSpotById(spotId)||GRIND_SPOTS[0],defaults=grindSessionDefaults();if(!spot)return;if(grindEl.id)grindEl.id.value="";grindPopulateFormSpotSelect(grindEl.formSpotSearch?.value||"",String(spot.id));if(grindEl.hours)grindEl.hours.value=String(defaults.hours);if(grindEl.minutes)grindEl.minutes.value=String(defaults.minutes);grindSetClassValue(defaults.className);if(grindEl.dropRate)grindEl.dropRate.value=defaults.dropRate?String(defaults.dropRate):"";grindSetAgris(defaults.agris);grindSetBuffSelection(defaults.buffs);grindRenderLootInputs(spot,{});if(grindEl.formTitle)grindEl.formTitle.textContent="Add Session";if(grindEl.cancel)grindEl.cancel.style.display="none";if(stopDraft)grindSetReviewReady(false);grindRefreshEditingIndicators()}
function grindEditSession(id){const session=grindSessions().find(item=>item.id===id);if(!session)return;grindStopDraft();grindState.selectedSpotId=String(session.spotId);persistSetting("grindTrackerSelectedSpot",grindState.selectedSpotId);grindSetScreen("spot");grindSetReviewReady(true);if(grindEl.formSpotSearch)grindEl.formSpotSearch.value="";grindPopulateFormSpotSelect("",grindState.selectedSpotId);if(grindEl.id)grindEl.id.value=session.id;if(grindEl.hours)grindEl.hours.value=Math.floor((Number(session.minutes)||0)/60);if(grindEl.minutes)grindEl.minutes.value=(Number(session.minutes)||0)%60;grindSetClassValue(session.className||"");if(grindEl.dropRate)grindEl.dropRate.value=Number(session.dropRate)||0;grindSetAgris(!!session.agris);grindSetBuffSelection(session.buffs||[]);const counts={};(session.loot||[]).forEach(item=>counts[String(item.id)]=Number(item.count)||0);grindRenderLootInputs(grindSpotById(session.spotId),counts);if(grindEl.formTitle)grindEl.formTitle.textContent="Edit Session";if(grindEl.cancel)grindEl.cancel.style.display="inline-grid";grindRender();grindEl.form?.scrollIntoView({behavior:"smooth",block:"center"})}
async function grindSaveForm(event){event.preventDefault();const spotId=String(grindEl.formSpot?.value||grindState.selectedSpotId),hours=Math.max(0,Number(grindEl.hours?.value||0)),mins=Math.max(0,Math.min(59,Number(grindEl.minutes?.value||0))),minutes=Math.round(hours*60+mins),id=grindEl.id?.value||`grind-${Date.now()}-${Math.random().toString(36).slice(2,7)}`;if(minutes<=0){NotificationService.ShowWarning("Enter a session duration greater than zero.","Grind Tracker");grindEl.hours?.focus();return;}await grindEnsureMarketPricesForSpot(spotId);const rows=grindSessions().slice(),index=rows.findIndex(item=>item.id===id),previous=index>=0?rows[index]:null,date=previous?.date||grindLocalDateTimeStamp(),className=String(grindEl.className?.value||"").trim(),buffs=grindSelectedBuffIds(),preservedLoot=previous&&String(previous.spotId)===spotId?previous.loot||[]:[],loot=grindPriceLootItems(grindCollectLoot(),grindSpotById(spotId),preservedLoot),silver=grindLootValue(loot),priceCapturedAt=previous?.priceCapturedAt||new Date().toISOString(),pricingIncomplete=loot.some(item=>String(item.priceSource||"")==="missing");const session={id,spotId,date,minutes,silver,className,dropRate:Math.max(0,Number(grindEl.dropRate?.value||0)),agris:grindCurrentAgris(),buffs,loot,priceRegion:previous?.priceRegion||grindState.marketRegion,priceCapturedAt,pricingIncomplete,notes:String(previous?.notes||""),source:previous?.source||"manual-review",updatedAt:grindLocalDateTimeStamp()};if(index>=0)rows[index]=session;else rows.unshift(session);try{await saveGrindSessions(rows)}catch(error){NotificationService.ShowError(error.message||"Could not save the session.","Grind Tracker");return}grindState.selectedSpotId=spotId;grindState.spotSessionPage=0;persistSetting("grindTrackerSelectedSpot",spotId);grindPersistCurrentSessionDefaults();grindResetForm(spotId);grindSetScreen("spot");grindSetReviewReady(false);grindRender();if(pricingIncomplete)NotificationService.ShowWarning(`Session saved at ${grindFormatSilver(silver)}, but one or more loot prices were unavailable.`,"Grind Tracker");else NotificationService.ShowSuccess(`Grind session saved: ${grindFormatSilver(silver)} silver.`,"Grind Tracker")}

const eventsState={initialized:false,loading:false,events:[],selectedId:"",lastStatus:""};
const eventsEl={status:document.getElementById("eventsStatusText"),refresh:document.getElementById("eventsRefresh"),timelineTitle:document.getElementById("eventsTimelineTitle"),timelineCount:document.getElementById("eventsTimelineCount"),timelineDays:document.getElementById("eventsTimelineDays"),timelineBars:document.getElementById("eventsTimelineBars"),detail:document.getElementById("eventsDetail")};
function eventCategorySlug(category){return String(category||"Adventure").toLowerCase().replace(/[^a-z0-9]+/g,"-").replace(/^-|-$/g,"")||"adventure"}
function eventDateText(value){const date=new Date(value);return Number.isNaN(date.getTime())?"":date.toLocaleDateString([],{month:"short",day:"numeric"})}
function eventDateValue(value){const date=new Date(value);return Number.isNaN(date.getTime())?null:date}
function eventDayStart(value){const date=value instanceof Date?new Date(value):eventDateValue(value);if(!date)return null;date.setHours(0,0,0,0);return date}
function eventMonthRangeText(days){if(!days.length)return"Official BDO Events";const first=days[0],last=days[days.length-1],firstMonth=first.toLocaleDateString([],{month:"long"}),lastMonth=last.toLocaleDateString([],{month:"long"}),year=last.getFullYear();return firstMonth===lastMonth?`${firstMonth} ${year}`:`${firstMonth} - ${lastMonth} ${year}`}
function eventTimelineLabel(title){const value=String(title||"Official event").replace(/\s+/g," ").trim();return value.length>34?`${value.slice(0,33).trim()}...`:value}
function eventCompactTimeLeft(event){let hours=Number(event?.remainingHours);if(!Number.isFinite(hours)){const end=eventDateValue(event?.endUtc);hours=end?Math.ceil((end-new Date())/3600000):NaN}if(!Number.isFinite(hours)||hours<=0)return"Ends soon";if(hours<24)return`${Math.max(1,Math.round(hours))}h`;const days=Math.floor(hours/24),remaining=Math.round(hours%24);return remaining>0?`${days}d${remaining}h`:`${days}d`}
function eventNowPercent(firstDay,totalDays){const now=new Date(),dayMs=86400000;return eventClamp(((now-firstDay)/dayMs)/totalDays*100,0,100)}
function eventClockText(){return new Date().toLocaleTimeString([],{hour:"2-digit",minute:"2-digit",second:"2-digit"})}
const eventTimelinePalette=["#43c47d","#d85d61","#3f95d8","#d08f34","#c25a91","#58b7c8","#8f72e6","#d0b640","#de7a3a","#65aa58","#e05a7a","#4da0b8"];
function eventStableHash(value){let hash=0;for(const char of String(value||""))hash=(hash*31+char.charCodeAt(0))>>>0;return hash}
function eventTimelineColor(event,index){return eventTimelinePalette[(eventStableHash(event?.title||event?.id)+index)%eventTimelinePalette.length]}
function eventClamp(value,min,max){return Math.max(min,Math.min(max,value))}
function eventTimelinePosition(event,firstDay,totalDays){const dayMs=86400000;let start=eventDayStart(event.startUtc)||eventDayStart(event.publishedUtc);let end=eventDayStart(event.endUtc);if(!start&&end){const remainingDays=Math.max(1,Math.min(12,Math.ceil((Number(event.remainingHours)||24)/24)));start=new Date(end);start.setDate(start.getDate()-remainingDays)}if(start&&!end){end=new Date(start);end.setDate(end.getDate()+3)}if(!start&&!end){start=new Date(firstDay);end=new Date(firstDay)}if(end<firstDay)return null;const rawStart=Math.floor((start-firstDay)/dayMs)+1,rawEnd=Math.floor((end-firstDay)/dayMs)+2;if(rawStart>totalDays+1)return null;const startColumn=eventClamp(rawStart,1,totalDays),endColumn=eventClamp(Math.max(rawEnd,startColumn+1),startColumn+1,totalDays+1);return{start:startColumn,end:endColumn}}
function eventTimeLeftText(event){if(event.timeLeftText)return event.timeLeftText;const hours=Number(event.remainingHours);if(!Number.isFinite(hours))return "Active";if(hours<24)return `${Math.max(1,Math.round(hours))}h left`;const days=Math.ceil(hours/24);return `${days}d left`}
function eventSummary(event){return event.summary||"Open the official event page for full rewards, rules, and schedule details."}
function eventImage(event){return event.imageUrl||""}
function eventCssImage(value){return String(value||"").replace(/\\/g,"/").replace(/'/g,"%27").replace(/\(/g,"%28").replace(/\)/g,"%29")}
function setEventsStatus(message,error=false){if(eventsEl.status){eventsEl.status.textContent=message;eventsEl.status.closest(".eventsStatus")?.classList.toggle("error",error)}}
function eventGroups(){const sorted=eventsState.events.slice().sort((a,b)=>(a.remainingHours??999999)-(b.remainingHours??999999)||String(a.title).localeCompare(String(b.title)));return{ending:sorted.filter(event=>event.status==="endingSoon"),active:sorted.filter(event=>event.status!=="endingSoon")}}
function renderEventTimeline(){if(!eventsEl.timelineDays||!eventsEl.timelineBars)return;const today=eventDayStart(new Date()),firstDay=new Date(today);firstDay.setDate(firstDay.getDate()-5);const totalDays=21,days=[];eventsEl.timelineDays.style.gridTemplateColumns=`repeat(${totalDays}, minmax(64px, 1fr))`;eventsEl.timelineBars.style.gridTemplateColumns=`repeat(${totalDays}, minmax(64px, 1fr))`;for(let i=0;i<totalDays;i++){const date=new Date(firstDay);date.setDate(firstDay.getDate()+i);days.push(date)}eventsEl.timelineDays.innerHTML=days.map(date=>`<div class="eventsTimelineDay ${date.getTime()===today.getTime()?"today":""}"><span>${date.toLocaleDateString([],{weekday:"short"})}</span><strong>${date.getDate()}</strong></div>`).join("");if(eventsEl.timelineTitle)eventsEl.timelineTitle.textContent=eventMonthRangeText(days);if(eventsEl.timelineCount)eventsEl.timelineCount.textContent=`${eventsState.events.length} official event${eventsState.events.length===1?"":"s"}`;const timelineEvents=eventsState.events.slice().sort((a,b)=>(a.remainingHours??999999)-(b.remainingHours??999999)||(eventDateValue(a.endUtc)||new Date(8640000000000000))-(eventDateValue(b.endUtc)||new Date(8640000000000000))||String(a.title).localeCompare(String(b.title)));const bars=[];for(const event of timelineEvents){const position=eventTimelinePosition(event,firstDay,totalDays);if(!position)continue;const row=bars.length+1,image=eventImage(event),color=eventTimelineColor(event,row),style=`--event-start:${position.start};--event-end:${position.end};--event-lane:${row};--event-color:${color};${image?`--event-image:url('${eventCssImage(image)}');`:""}`;bars.push(`<button class="eventsTimelineBar ${event.id===eventsState.selectedId?"selected":""}" style="${style}" data-event-id="${escapeHtml(event.id)}" data-event-category="${eventCategorySlug(event.category)}" title="${escapeHtml(event.title)}">${image?`<span class="eventsTimelineArt" aria-hidden="true"></span>`:""}<span class="eventsTimelineBarText">${escapeHtml(eventTimelineLabel(event.title))}</span><span class="eventsTimelinePill">${escapeHtml(eventCompactTimeLeft(event))}</span></button>`)}const nowLine=`<div class="eventsNowLine" style="--now-pos:${eventNowPercent(firstDay,totalDays)}%"><span>${escapeHtml(eventClockText())}</span></div>`;eventsEl.timelineBars.innerHTML=nowLine+(bars.join("")||`<div class="eventsEmpty" style="grid-column:1/-1">Official events will appear here after the first refresh.</div>`)}
function updateEventTimelineClock(){const nowLine=eventsEl.timelineBars?.querySelector(".eventsNowLine"),today=eventDayStart(new Date()),todayLabel=eventsEl.timelineDays?.querySelector(".eventsTimelineDay.today strong")?.textContent;if(!nowLine||todayLabel!==String(today.getDate())){renderEventTimeline();return;}const firstDay=new Date(today);firstDay.setDate(firstDay.getDate()-5);nowLine.style.setProperty("--now-pos",`${eventNowPercent(firstDay,21)}%`);const label=nowLine.querySelector("span");if(label)label.textContent=eventClockText()}
function renderEventDetail(){if(!eventsEl.detail)return;const event=eventsState.events.find(item=>item.id===eventsState.selectedId)||eventsState.events[0];if(!event){eventsEl.detail.innerHTML=`<div class="eventsEmpty">Official BDO events will appear here once loaded.</div>`;return;}const image=eventImage(event),category=escapeHtml(event.category||"Adventure"),timeLeft=escapeHtml(eventTimeLeftText(event));eventsEl.detail.innerHTML=`<div class="eventsDetailHero">${image?`<img src="${escapeHtml(image)}" alt="">`:""}<div class="eventsDetailHeroText"><span class="eventBadge" data-event-category="${eventCategorySlug(event.category)}">${category}</span><h2>${escapeHtml(event.title)}</h2></div></div><div class="eventsDetailBody"><div class="eventsDetailMeta"><span>${escapeHtml(event.dateRangeText||eventDateText(event.endUtc)||"Official schedule")}</span><span>${timeLeft}</span><span>${escapeHtml(event.source||"Official BDO")}</span></div><p class="eventsDetailSummary">${escapeHtml(eventSummary(event))}</p><button class="eventsOpen" data-open-url="${escapeHtml(event.url)}" type="button">Open Official Event &nbsp; &nearr;</button></div>`}
function renderEvents(){const groups=eventGroups();if(eventsEl.timelineCount)eventsEl.timelineCount.textContent=`${eventsState.events.length} official event${eventsState.events.length===1?"":"s"}`;if(eventsEl.timelineTitle)eventsEl.timelineTitle.textContent="Official BDO Events";if(!eventsState.selectedId||!eventsState.events.some(event=>event.id===eventsState.selectedId))eventsState.selectedId=(groups.ending[0]||groups.active[0]||eventsState.events[0]||{}).id||"";renderEventTimeline();renderEventDetail()}
function eventCacheAgeText(minutes){const value=Number(minutes);if(!Number.isFinite(value)||value<=0)return"";if(value<60)return`${Math.max(1,Math.round(value))}m old`;const hours=Math.round(value/60);return hours<48?`${hours}h old`:`${Math.round(hours/24)}d old`}
function applyEventsDashboard(data){eventsState.events=Array.isArray(data.events)?data.events:[];eventsState.lastStatus=String(data.status||"CACHED").toUpperCase();const date=new Date(data.lastAttempt||data.lastRefreshed);const time=Number.isNaN(date.getTime())?"":` - Last checked ${date.toLocaleTimeString([],{hour:"2-digit",minute:"2-digit"})}`;const cachedLabel=data.isStale?`Cached official events loaded${data.cacheAgeMinutes?` (${eventCacheAgeText(data.cacheAgeMinutes)})`:""}`:"Cached official events loaded";setEventsStatus(data.message||`${eventsState.lastStatus==="LIVE"?"Official events synced":cachedLabel}${time}`,Boolean(data.message));renderEvents()}
async function loadEvents(force=false){if(eventsState.loading)return;eventsState.loading=true;if(eventsEl.refresh){eventsEl.refresh.disabled=true;eventsEl.refresh.textContent=force?"Refreshing...":"Loading..."}setEventsStatus(force?"Refreshing official BDO events...":"Loading official BDO events...");try{const data=await bridgeCall(force?"refreshEvents":"initializeEvents");applyEventsDashboard(data);if(!force&&data?.isStale&&Array.isArray(data.events)&&data.events.length)setTimeout(()=>{if(!eventsState.loading)loadEvents(true)},700);if(force&&String(data.status||"").toUpperCase()==="LIVE")NotificationService.ShowSuccess("Official BDO events refreshed.","Events");else if(force&&data.message)NotificationService.ShowWarning(data.message,"Events");}catch(error){setEventsStatus(error.message||"Could not load official events.",true);NotificationService.ShowError(error.message||"Could not load official BDO events.");}finally{eventsState.loading=false;if(eventsEl.refresh){eventsEl.refresh.disabled=false;eventsEl.refresh.textContent="Refresh Events"}}}
function initializeEvents(){if(!eventsState.initialized){eventsState.initialized=true;eventsEl.refresh?.addEventListener("click",()=>loadEvents(true));eventsEl.timelineBars?.addEventListener("click",event=>{const item=event.target.closest("[data-event-id]");if(!item)return;eventsState.selectedId=item.dataset.eventId;renderEvents()})}renderEvents();loadEvents(false)}
initializeAppVersion();
initializeHomeDashboard();
setTimeout(()=>initializeCoupons(),1000);
setTimeout(()=>initializeUpdateChecker(),1600);
document.addEventListener("click", async event => {
  const external = event.target.closest("[data-open-url]");
  if(!external || external.closest("#couponDetail")) return;
  try {
    await bridgeCall("openExternalUrl", { url: external.dataset.openUrl });
  } catch(error) {
    NotificationService.ShowError(error.message || "Could not open link.");
  }
});

let appViewTransitionTimer=null;
let activeAppViewId=document.querySelector(".appView.active")?.id||"homeView";
const CINEMATIC_BACKGROUNDS=["homeView","calculatorView","marketView","portraitView","fontChangerView","couponsView","eventsView","grindTrackerView","settingsView","resetTimersView","bracketsView","masteryBracketsView"].reduce((map,view,index)=>{map[view]=`Assets/CinematicBackgrounds/cinematic-${String(Math.min(index+1,10)).padStart(2,"0")}.jpg`;return map;},{lightstoneSetsView:"Assets/CinematicBackgrounds/cinematic-10.jpg"});
function updateCinematicBackground(viewId){const url=CINEMATIC_BACKGROUNDS[viewId]||CINEMATIC_BACKGROUNDS.homeView;document.body.style.setProperty("--cinematic-bg",`url("${url}")`)}
function tickActiveAppView(){if(document.hidden)return;if(activeAppViewId==="homeView")updateHomeTimers(normalizedHomeSettings());else if(activeAppViewId==="resetTimersView")renderResetTimers(normalizedResetSettings());else if(activeAppViewId==="eventsView"&&eventsState.events.length)updateEventTimelineClock()}
clearInterval(window.__bdoActiveViewTicker);
window.__bdoActiveViewTicker=setInterval(tickActiveAppView,1000);
function syncPageVisibility(){document.body.classList.toggle("appHidden",document.hidden);if(!document.hidden)tickActiveAppView()}
document.addEventListener("visibilitychange",syncPageVisibility);
syncPageVisibility();
function syncFixedChromeOffset(){
  const title=document.getElementById("windowTitleBar");
  const nav=document.querySelector(".navFrame");
  const titleHeight=Math.ceil(title?.getBoundingClientRect().height||62);
  const navHeight=Math.ceil(nav?.getBoundingClientRect().height||150);
  document.documentElement.style.setProperty("--titleBarHeight",`${titleHeight}px`);
  document.documentElement.style.setProperty("--fixedTopOffset",`${titleHeight+navHeight+18}px`);
}
function initializeAppView(viewId){
  activeAppViewId=viewId;
  updateCinematicBackground(viewId);
  if(viewId === "homeView") initializeHomeDashboard();
  if(viewId === "marketView") initializeMarket();
  if(viewId === "portraitView") initializePortraitReplacer();
  if(viewId === "fontChangerView") initializeFontChanger();
  if(viewId === "couponsView") initializeCoupons();
  if(viewId === "eventsView") initializeEvents();
  if(viewId === "grindTrackerView") initializeGrindTracker();
  if(viewId === "resetTimersView") initializeResetTimers();
  if(viewId === "bracketsView") initializeBrackets();
  if(viewId === "masteryBracketsView") initializeMasteryBrackets();
  if(viewId === "lightstoneSetsView") initializeLightstoneSets();
}
function activateAppView(button){
  const targetId=button.dataset.appView,current=document.querySelector(".appView.active"),target=document.getElementById(targetId);
  if(!target||current===target)return;
  clearTimeout(appViewTransitionTimer);
  document.querySelectorAll("[data-app-view]").forEach(x => x.classList.toggle("active", x === button));
  if(current){
    current.classList.add("viewFading");
    appViewTransitionTimer=setTimeout(()=>{
      current.classList.remove("active","viewFading");
      target.classList.add("active","viewFading");
      initializeAppView(targetId);
      syncFixedChromeOffset();
      requestAnimationFrame(()=>target.classList.remove("viewFading"));
    },180);
  }else{
    target.classList.add("active");
    initializeAppView(targetId);
    syncFixedChromeOffset();
  }
}
document.querySelectorAll("[data-app-view]").forEach(button => {
  button.addEventListener("click", () => activateAppView(button));
});
window.addEventListener("resize", syncFixedChromeOffset);
requestAnimationFrame(syncFixedChromeOffset);
setTimeout(syncFixedChromeOffset,250);

const titleBar = document.getElementById("windowTitleBar");
titleBar?.addEventListener("pointerdown", event => {
  if(event.button !== 0 || event.target.closest("button")) return;
  bridgeCall("windowDrag").catch(() => {});
});
titleBar?.addEventListener("dblclick", event => {
  if(event.target.closest("button")) return;
  bridgeCall("windowToggleMaximize").catch(() => {});
});
document.getElementById("windowMinimize")?.addEventListener("click", () => bridgeCall("windowMinimize").catch(() => {}));
document.getElementById("windowMaximize")?.addEventListener("click", () => bridgeCall("windowToggleMaximize").catch(() => {}));
document.getElementById("windowClose")?.addEventListener("click", () => bridgeCall("windowClose").catch(() => {}));

document.querySelectorAll("[data-market-panel]").forEach(button => {
  button.addEventListener("click", () => {
    document.querySelectorAll("[data-market-panel]").forEach(x => x.classList.toggle("active", x === button));
    document.querySelectorAll(".marketPanelView").forEach(panel =>
      panel.classList.toggle("active", panel.id === button.dataset.marketPanel));
    if(button.dataset.marketPanel === "outfitPanel") loadOutfitReport();
  });
});

let marketSearchTimer;
let marketSearchGeneration=0;
marketEl.search.addEventListener("input", () => {
  clearTimeout(marketSearchTimer);
  const generation=++marketSearchGeneration;
  const query = marketEl.search.value.trim();
  if(query.length < 2) {
    marketEl.searchResults.innerHTML = "";
    return;
  }
  marketSearchTimer = setTimeout(() => searchMarket(query,generation), 350);
});

async function searchMarket(query,generation=++marketSearchGeneration) {
  try {
    marketEl.searchResults.innerHTML = `<div class="marketSearchResult">Searching...</div>`;
    const results = await bridgeCall("search", {query});
    if(generation!==marketSearchGeneration)return;
    marketEl.searchResults.innerHTML = results.slice(0, 80).map(item => `
      <button class="marketSearchResult" data-item-id="${item.itemId}">
        <span><strong>${escapeHtml(item.name)}</strong><small>Item ${item.itemId}</small></span>
        <small>${fmtSilver(item.currentPrice)}</small><small>${fmtInt(item.stock)} listed</small>
      </button>`).join("") || `<div class="marketSearchResult">No matching market items.</div>`;
  } catch(error) {
    if(generation!==marketSearchGeneration)return;
    marketEl.searchResults.innerHTML = `<div class="marketSearchResult negative">${escapeHtml(error.message)}</div>`;
  }
}

marketEl.searchResults.addEventListener("click", async event => {
  const button = event.target.closest("[data-item-id]");
  if(!button) return;
  const generation=++marketSearchGeneration;
  try {
    setMarketStatus("Loading item options...");
    const variants = await bridgeCall("getVariants", {itemId:Number(button.dataset.itemId)});
    if(generation!==marketSearchGeneration)return;
    marketEl.searchResults.innerHTML = variants.map(item => `
      <button class="marketSearchResult" data-add-item='${escapeHtml(JSON.stringify(item))}'>
        <span><strong>${escapeHtml(enhancedName(item))}</strong><small>Click to track</small></span>
        <small>${fmtSilver(item.currentPrice)}</small><small>${fmtInt(item.tradeCount)} lifetime sales</small>
      </button>`).join("");
  } catch(error) {
    setMarketStatus(error.message, true);
  }
});

marketEl.searchResults.addEventListener("click", async event => {
  const button = event.target.closest("[data-add-item]");
  if(!button) return;
  try {
    const item = JSON.parse(button.dataset.addItem);
    setMarketStatus(`Adding ${enhancedName(item)}...`);
    marketState.items = await bridgeCall("addTracked", item);
    marketEl.search.value = "";
    marketEl.searchResults.innerHTML = "";
    renderTrackedItems();
    selectTrackedItem(marketState.items.find(x => x.itemId === item.itemId && x.enhancement === item.enhancement));
    setMarketStatus("Item added and initial snapshot stored.");
  } catch(error) {
    setMarketStatus(error.message, true);
  }
});

function enhancedName(item) {
  return item.enhancement > 0 ? `${item.name} +${item.enhancement}` : item.name;
}

function renderTrackedItems() {
  const filter = norm(marketEl.trackedFilter.value);
  const sort = marketEl.trackedSort.value;
  let items = marketState.items.filter(item => !filter || norm(enhancedName(item)).includes(filter));
  items = [...items].sort((a,b) => {
    if(sort === "priceDesc") return (b.lastPrice || 0) - (a.lastPrice || 0);
    if(sort === "updated") return String(b.lastUpdatedUtc || "").localeCompare(String(a.lastUpdatedUtc || ""));
    return enhancedName(a).localeCompare(enhancedName(b));
  });
  marketEl.trackedCount.textContent = String(marketState.items.length);
  marketEl.trackedItems.innerHTML = items.map(item => `
    <button class="trackedItem ${marketState.selected?.itemId === item.itemId && marketState.selected?.enhancement === item.enhancement ? "active" : ""}"
      data-tracked-id="${item.itemId}" data-enhancement="${item.enhancement}">
      <strong>${escapeHtml(enhancedName(item))}</strong>
      <span>${fmtSilver(item.lastPrice)} | ${item.lastUpdatedUtc ? new Date(item.lastUpdatedUtc).toLocaleString() : "Not updated"}</span>
    </button>`).join("") || `<div class="marketEmpty" style="min-height:180px">No tracked items match.</div>`;
}

marketEl.trackedFilter.addEventListener("input", renderTrackedItems);
marketEl.trackedSort.addEventListener("change", renderTrackedItems);
marketEl.trackedItems.addEventListener("click", event => {
  const button = event.target.closest("[data-tracked-id]");
  if(!button) return;
  selectTrackedItem(marketState.items.find(item =>
    item.itemId === Number(button.dataset.trackedId) && item.enhancement === Number(button.dataset.enhancement)));
});

function selectTrackedItem(item) {
  if(!item) return;
  marketState.selected = item;
  renderTrackedItems();
  loadAnalytics();
}

async function loadAnalytics() {
  if(!marketState.selected) return;
  try {
    setMarketStatus("Loading item analytics...");
    marketState.analytics = await bridgeCall("getAnalytics", {
      itemId:marketState.selected.itemId,
      enhancement:marketState.selected.enhancement,
      region:getMarketRegion(),
      days:Number(marketEl.range.value)
    });
    renderAnalytics();
    setMarketStatus("Ready");
  } catch(error) {
    setMarketStatus(error.message, true);
  }
}

function renderAnalytics() {
  const data = marketState.analytics;
  if(!data) return clearMarketDetail();
  marketEl.empty.hidden = true;
  marketEl.detail.hidden = false;
  marketEl.detailName.textContent = enhancedName(data.item);
  marketEl.detailMeta.textContent =
    `${data.item.region.toUpperCase()} | Item ${data.item.itemId} | Last update ${data.item.lastUpdatedUtc ? new Date(data.item.lastUpdatedUtc).toLocaleString() : "pending"}`;
  marketEl.current.textContent = fmtSilver(data.currentPrice);
  marketEl.min.textContent = fmtSilver(data.minimumPrice);
  marketEl.max.textContent = fmtSilver(data.maximumPrice);
  marketEl.average.textContent = fmtSilver(data.averagePrice);
  marketEl.trend.textContent = data.trendPercent == null ? "Building history" : `${data.trendPercent >= 0 ? "+" : ""}${data.trendPercent.toFixed(2)}%`;
  marketEl.trend.className = data.trendPercent == null ? "" : data.trendPercent >= 0 ? "positive" : "negative";
  marketEl.salesGrid.innerHTML = data.sales.map(window => `
    <div class="marketMetric">
      <span>Sales: ${escapeHtml(window.label)}</span>
      <strong>${window.complete ? fmtInt(window.sales) : "Building history"}</strong>
      <small class="confidence">${window.complete ? `${window.coverageHours.toFixed(0)}h local coverage` : `${window.coverageHours.toFixed(0)}h collected`}</small>
    </div>`).join("");
  drawLineChart(marketEl.priceChart, data.points.map(point => ({time:new Date(point.timestamp), value:point.price})), fmtSilver);
  const salesPoints = [];
  const tradePoints = data.points.filter(point => point.tradeCount != null);
  for(let i=1; i<tradePoints.length; i++) {
    salesPoints.push({
      time:new Date(tradePoints[i].timestamp),
      value:Math.max(0, tradePoints[i].tradeCount - tradePoints[i-1].tradeCount)
    });
  }
  drawLineChart(marketEl.salesChart, salesPoints, value => fmtInt(value));
}

function clearMarketDetail() {
  marketState.selected = null;
  marketState.analytics = null;
  marketEl.empty.hidden = false;
  marketEl.detail.hidden = true;
  renderTrackedItems();
}

marketEl.range.addEventListener("change", loadAnalytics);
marketEl.remove.addEventListener("click", async () => {
  if(!marketState.selected) return;
  try {
    marketState.items = await bridgeCall("removeTracked", {
      itemId:marketState.selected.itemId,
      enhancement:marketState.selected.enhancement
    });
    clearMarketDetail();
    setMarketStatus("Item removed from tracking. Existing history remains available in CSV exports.");
  } catch(error) {
    setMarketStatus(error.message, true);
  }
});

async function loadMarketRegionState(region = getMarketRegion(), updateStatus = true) {
  try {
    const selectedRegion = "eu";
    const requestNumber = ++marketState.outfitRequestNumber;
    const state = await bridgeCall("getRegionState", { region:selectedRegion });
    if(requestNumber !== marketState.outfitRequestNumber || getMarketRegion() !== selectedRegion) return;
    marketState.items = state.items || [];
    marketState.outfits = state.outfits || null;
    clearMarketDetail();
    renderTrackedItems();
    renderOutfitReport();
    if(updateStatus) setMarketStatus(`${selectedRegion.toUpperCase()} cached samples loaded.`);
  } catch(error) {
    setMarketStatus(error.message, true);
  }
}

marketEl.regionButtons.forEach(button => {
  button.addEventListener("click", async () => {
    const nextRegion = button.dataset.marketRegion || "eu";
    if(getMarketRegion() === nextRegion) return;
    const panels = marketRegionPanels();
    try {
      panels.forEach(panel => panel.classList.add("marketRegionFading"));
      await waitForMarketFade();
      setMarketRegion(nextRegion);
      await loadMarketRegionState(nextRegion);
    } finally {
      requestAnimationFrame(() => panels.forEach(panel => panel.classList.remove("marketRegionFading")));
    }
  });
});
marketEl.export.addEventListener("click", async () => {
  try {
    const result = await bridgeCall("exportCsv");
    setMarketStatus(result.cancelled ? "Export cancelled." : `CSV exported to ${result.path}`);
  } catch(error) {
    setMarketStatus(error.message, true);
  }
});

async function loadOutfitReport() {
  if(!marketState.initialized) return;
  const selectedRegion = getMarketRegion();
  const requestNumber = ++marketState.outfitRequestNumber;
  try {
    const report = await bridgeCall("getOutfitReport", { region:selectedRegion });
    if(requestNumber !== marketState.outfitRequestNumber || getMarketRegion() !== selectedRegion) return;
    marketState.outfits = report;
    renderOutfitReport();
  } catch(error) {
    if(requestNumber !== marketState.outfitRequestNumber) return;
    marketEl.outfitCoverage.textContent = error.message;
  }
}

function renderOutfitReport() {
  const report = marketState.outfits;
  if(!report) return;
  const selectedRegionLabel = getMarketRegion().toUpperCase();
  marketEl.outfitCoverage.textContent =
    `${fmtInt(report.catalogCount)} outfits discovered | ${fmtInt(report.detailedCount)} with detailed local samples | ${report.coveragePercent.toFixed(1)}% coverage`;
  const filter = norm(marketEl.outfitFilter.value)
    .replace(/\bberzerker\b/g, "berserker")
    .replace(/\bzerker\b/g, "berserker");
  const filtered = report.opportunities.filter(item => !filter || norm(item.name).includes(filter));
  const recommendationRank = item => {
    const chance = item.sevenDayChancePercent == null ? 0 : item.sevenDayChancePercent;
    const preorder = item.preorderCount == null ? 0 : item.preorderCount;
    const sales = item.sales7Days == null ? 0 : item.sales7Days;
    const priceWeight = item.price ? Math.log10(Math.max(10, item.price)) : 0;
    const stockPenalty = item.stock == null ? 0 : Math.min(5, Math.log10(Math.max(1, item.stock)));
    return (item.score || 0) * 1000 + chance * 2 + Math.log10(preorder + 1) * 25 + sales * 4 + priceWeight * 3 - stockPenalty;
  };
  const topThree = [...filtered]
    .filter(item => item.sampleCount > 0 || item.preorderCount != null || item.price > 0)
    .sort((a,b) =>
      (b.recommendationEligible === true) - (a.recommendationEligible === true) ||
      recommendationRank(b) - recommendationRank(a) ||
      (b.preorderCount || 0) - (a.preorderCount || 0) ||
      (b.price || 0) - (a.price || 0) ||
      a.name.localeCompare(b.name))
    .slice(0, 3);
  const sampleReadyCount = filtered.filter(item => item.sampleCount >= 5).length;
  const windowReadyCount = filtered.filter(item =>
    item.sales24Hours != null &&
    item.sales3Days != null &&
    item.sales7Days != null
  ).length;
  marketEl.topOutfitCards.innerHTML = topThree.map((item,index) => {
    const signal = item.recommendationEligible ? "Strong preorder signal" : `Best available ${selectedRegionLabel} market signal`;
    return `<article class="mustOrderCard">
      <div class="mustOrderRank">Must order #${index + 1}</div>
      <h3>${escapeHtml(item.name)}</h3>
      <div class="mustOrderStats">
        <span>Recommendation<strong>${signal}</strong></span>
        <span>Price<strong>${fmtSilver(item.price)}</strong></span>
        <span>24h / 3d / 7d<strong>${[item.sales24Hours,item.sales3Days,item.sales7Days].map(x => x == null ? "-" : fmtInt(x)).join(" / ")}</strong></span>
        <span>Preorders<strong>${item.preorderCount == null ? "Scanning" : fmtInt(item.preorderCount)}</strong></span>
        <span>Queue estimate<strong>${item.estimatedQueueDays == null ? "-" : item.estimatedQueueDays < 1 ? "< 1 day" : `${item.estimatedQueueDays.toFixed(1)} days`}</strong></span>
      </div>
    </article>`;
  }).join("") || `<div class="mustOrderCard">
    <strong>No active outfit recommendations yet</strong>
    <span class="confidence">${fmtInt(sampleReadyCount)} outfits have 5+ samples | ${fmtInt(windowReadyCount)} have complete windows. Waiting for stronger ${selectedRegionLabel} sales movement.</span>
  </div>`;
  const rows = filtered.slice(0, 500);
  marketEl.outfitRows.innerHTML = rows.map((item,index) => {
    const queue = item.estimatedQueueDays == null ? "-" :
      item.estimatedQueueDays < 1 ? "< 1d" : `${item.estimatedQueueDays.toFixed(1)}d`;
    const momentum = item.demandMomentumPercent == null ? "" :
      `<span class="confidence">${item.demandMomentumPercent >= 0 ? "+" : ""}${item.demandMomentumPercent.toFixed(0)}% recent momentum</span>`;
    return `<tr>
      <td>${index + 1}</td>
      <td><strong>${escapeHtml(item.name)}</strong><span class="confidence">Item ${item.itemId}</span></td>
      <td class="right mono">${item.sales24Hours == null ? "-" : fmtInt(item.sales24Hours)}</td>
      <td class="right mono">${item.sales3Days == null ? "-" : fmtInt(item.sales3Days)}</td>
      <td class="right mono">${item.sales7Days == null ? "-" : fmtInt(item.sales7Days)}</td>
      <td class="right mono">${item.preorderCount == null ? "-" : fmtInt(item.preorderCount)}</td>
      <td class="right mono">${queue}${momentum}</td>
      <td class="right mono">${fmtSilver(item.price)}</td>
    </tr>`;
  }).join("") || `<tr><td colspan="8">No outfits match this filter.</td></tr>`;
}

marketEl.outfitFilter.addEventListener("input", renderOutfitReport);

function fmtSilver(value) {
  if(value == null || !Number.isFinite(Number(value))) return "-";
  const number = Number(value);
  if(number >= 1e9) return `${(number / 1e9).toFixed(2)}b`;
  if(number >= 1e6) return `${(number / 1e6).toFixed(2)}m`;
  if(number >= 1e3) return `${(number / 1e3).toFixed(1)}k`;
  return Math.round(number).toLocaleString();
}

function drawLineChart(canvas, points, valueFormatter) {
  const parent = canvas.parentElement;
  const tooltip = parent.querySelector(".graphTooltip");
  const width = Math.max(320, Math.floor(canvas.clientWidth));
  const height = 230;
  const ratio = window.devicePixelRatio || 1;
  canvas.width = width * ratio;
  canvas.height = height * ratio;
  const ctx = canvas.getContext("2d");
  ctx.scale(ratio, ratio);
  ctx.clearRect(0, 0, width, height);
  const style = getComputedStyle(document.body);
  const muted = style.getPropertyValue("--muted").trim();
  const line = style.getPropertyValue("--a1").trim();
  const grid = style.getPropertyValue("--border").trim();
  const plot = {left:48, top:14, right:width - 12, bottom:height - 28};

  ctx.strokeStyle = grid;
  ctx.fillStyle = muted;
  ctx.font = "10px Inter";
  for(let i=0; i<=4; i++) {
    const y = plot.top + (plot.bottom - plot.top) * i / 4;
    ctx.beginPath(); ctx.moveTo(plot.left, y); ctx.lineTo(plot.right, y); ctx.stroke();
  }
  if(!points.length) {
    ctx.fillText("History will appear after snapshots are collected.", plot.left, height / 2);
    canvas.onmousemove = null;
    tooltip.style.display = "none";
    return;
  }

  const values = points.map(x => Number(x.value));
  let minValue = Math.min(...values);
  let maxValue = Math.max(...values);
  if(minValue === maxValue) { minValue *= .98; maxValue *= 1.02; }
  const minTime = points[0].time.getTime();
  const maxTime = points[points.length - 1].time.getTime();
  const mapped = points.map((point,index) => ({
    ...point,
    x:plot.left + (plot.right - plot.left) * (maxTime === minTime ? index / Math.max(1, points.length - 1) : (point.time.getTime() - minTime) / (maxTime - minTime)),
    y:plot.bottom - (plot.bottom - plot.top) * (Number(point.value) - minValue) / Math.max(1, maxValue - minValue)
  }));
  ctx.fillText(valueFormatter(maxValue), 2, plot.top + 4);
  ctx.fillText(valueFormatter(minValue), 2, plot.bottom);
  ctx.fillText(points[0].time.toLocaleDateString(), plot.left, height - 8);
  const endLabel = points[points.length - 1].time.toLocaleDateString();
  ctx.fillText(endLabel, plot.right - ctx.measureText(endLabel).width, height - 8);
  ctx.strokeStyle = line;
  ctx.lineWidth = 2;
  ctx.beginPath();
  mapped.forEach((point,index) => index ? ctx.lineTo(point.x, point.y) : ctx.moveTo(point.x, point.y));
  ctx.stroke();

  canvas.onmousemove = event => {
    const bounds = canvas.getBoundingClientRect();
    const x = event.clientX - bounds.left;
    const nearest = mapped.reduce((best, point) => Math.abs(point.x - x) < Math.abs(best.x - x) ? point : best);
    tooltip.style.display = "block";
    tooltip.style.left = `${Math.min(width - 150, Math.max(6, nearest.x + 8))}px`;
    tooltip.style.top = `${Math.max(6, nearest.y - 42)}px`;
    tooltip.textContent = `${nearest.time.toLocaleString()} | ${valueFormatter(nearest.value)}`;
  };
  canvas.onmouseleave = () => tooltip.style.display = "none";
}

window.addEventListener("resize", () => {
  if(marketState.analytics) renderAnalytics();
});
